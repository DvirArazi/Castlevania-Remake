using System.Collections;
using SFML.Graphics;



interface IEntityGroup<out T> : IEnumerable<T> {
    public void EmptyKillStack();
}

class EntityGroup<T> : IEntityGroup<T> where T : Entity {
    public List<T> Entities;
    public Stack<T> KillStack;

    public EntityGroup() {
        Entities = new List<T>();
        KillStack = new Stack<T>();
    }

    public IEnumerator<T> GetEnumerator() {
        // return (IEnumerator<T>)Entities;
        foreach (var entity in Entities) {
            yield return entity;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }

    public void EmptyKillStack() {
        while (KillStack.Count > 0) {
            Entities.Remove(KillStack.Pop());
        }
    }
}