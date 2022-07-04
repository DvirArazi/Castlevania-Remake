using SFML.System;
using SFML.Graphics;

enum ItemType {
    Heart
} 

abstract class Item : Entity {
    static Texture TEXTURE; protected override Texture _texture {get => TEXTURE;}
    static Dictionary<ItemType, Section> SECTION_DICT;

    static Item() {
        TEXTURE = Textures.ITEMS;
        SECTION_DICT = new Dictionary<ItemType, Section>() {
            {ItemType.Heart, new Section(new IntRect(16, 0, 8, 8), new Vector2i(-4, -4))}
        };
    }

    public ItemType Type {get; private set;}
    protected bool _falling;
    float _fadeTime;

    public Item(Room room, Vector2f position, ItemType type) : base(room) {
        _position = position;
        _hitbox = (FloatRect)SECTION_DICT[Type].GetBox();
        Type = type;
        _falling = true;
        _fadeTime = 0f;
    }

    public override void Update(float elapsed) {
        if (_falling) {
            fall(elapsed);
        } else {
            fade(elapsed);
        }

        if (_fadeTime / 0.05f % 2f < 1f) {
            setSprite(SECTION_DICT[Type]);
        } else {
            setSprite(Global.EMPTY_SECTION);
        }
    }

    protected abstract void fall(float elapsed);

    void fade(float elapsed) {
        _fadeTime += elapsed;
        if (_fadeTime >= 3f) {
            Room.AddToItemKillStack(this);
        }
    }
}