namespace new_ConsoleDotNet
{
    public class LinkedList<T> 
    {
        private class Node
        {
            public T Value;
            public Node Next;
            public Node Prev;

            public Node(T value)
            {
                Value = value;
                Next = null;
                Prev = null;
            }
        }

        private Node head;
        private Node tail;
        public int Count { get; private set; }

        public LinkedList()
        {
            head = null;
            tail = null;
            Count = 0;
        }

        // AddFirst method
        public void AddFirst(T value)
        {
            Node newNode = new Node(value);
            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                newNode.Next = head;
                head.Prev = newNode;
                head = newNode;
            }
            Count++;
        }

        // AddLast method
        public void AddLast(T value)
        {
            Node newNode = new Node(value);
            if (tail == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
            Count++;
        }

        // AddBefore method
        public void AddBefore(T target, T value)
        {
            Node current = FindNode(target);
            if (current == null)
                throw new ArgumentException("Target not found in the list.");

            Node newNode = new Node(value);
            newNode.Next = current;
            newNode.Prev = current.Prev;

            if (current.Prev != null)
                current.Prev.Next = newNode;
            else
                head = newNode;

            current.Prev = newNode;
            Count++;
        }

        // AddAfter method
        public void AddAfter(T target, T value)
        {
            Node current = FindNode(target);
            if (current == null)
                throw new ArgumentException("Target not found in the list.");

            Node newNode = new Node(value);
            newNode.Prev = current;
            newNode.Next = current.Next;

            if (current.Next != null)
                current.Next.Prev = newNode;
            else
                tail = newNode;

            current.Next = newNode;
            Count++;
        }

        // Remove method
        public bool Remove(T value)
        {
            Node current = FindNode(value);
            if (current == null) return false;

            if (current.Prev != null)
                current.Prev.Next = current.Next;
            else
                head = current.Next;

            if (current.Next != null)
                current.Next.Prev = current.Prev;
            else
                tail = current.Prev;

            Count--;
            return true;
        }

        // Find method
        public bool Contains(T value)
        {
            return FindNode(value) != null;
        }

        private Node FindNode(T value)
        {
            Node current = head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, value))
                    return current;
                current = current.Next;
            }
            return null;


        }

        // Difference method
        public LinkedList<T> Difference(LinkedList<T> other)
        {
            var result = new LinkedList<T>();
            foreach (var item in this)
            {
                if (!other.Contains(item))
                    result.AddLast(item);
            }
            return result;
        }

        // Union method
        public LinkedList<T> Union(LinkedList<T> other)
        {
            var result = new LinkedList<T>();
            foreach (var item in this)
                result.AddLast(item);

            foreach (var item in other)
            {
                if (!result.Contains(item))
                    result.AddLast(item);
            }
            return result;
        }

        // Intersection method
        public LinkedList<T> Intersection(LinkedList<T> other)
        {
            var result = new LinkedList<T>();
            foreach (var item in this)
            {
                if (other.Contains(item))
                    result.AddLast(item);
            }
            return result;
        }

        // IEnumerable implementation for foreach
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }


        // Display the linked list (optional helper method)
        public void Print()
        {
            foreach (var item in this)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }

        

}
