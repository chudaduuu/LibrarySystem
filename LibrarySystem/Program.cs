// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem
{
    #region 枚举与基类
    enum ItemType { Novels, Magazine, TextBook }

    abstract class LibraryItem
    {
        public int Id { get; }
        public string Title { get; }
        public ItemType Type { get; }

        protected LibraryItem(int id, string title, ItemType type)
        {
            Id = id;
            Title = title;
            Type = type;
        }

        public abstract string GetDetails();
    }
    #endregion

    #region 具体物品类
    class Novel : LibraryItem
    {
        public string Author { get; }
        public Novel(int id, string title, string author)
            : base(id, title, ItemType.Novels) => Author = author;

        public override string GetDetails() => $"[Novel] {Title} by {Author}";
    }

    class Magazine : LibraryItem
    {
        public int IssueNumber { get; }
        public Magazine(int id, string title, int issue)
            : base(id, title, ItemType.Magazine) => IssueNumber = issue;

        public override string GetDetails() => $"[Magazine] {Title}, Issue #{IssueNumber}";
    }

    class TextBook : LibraryItem
    {
        public string Publisher { get; }
        public TextBook(int id, string title, string publisher)
            : base(id, title, ItemType.TextBook) => Publisher = publisher;

        public override string GetDetails() => $"[TextBook] {Title}, Publisher: {Publisher}";
    }
    #endregion

    #region 会员类
    class Member
    {
        public string Name { get; }
        private readonly List<LibraryItem> borrowedItems = new();

        public Member(string name) => Name = name;

        public string BorrowItem(LibraryItem item)
        {
            if (borrowedItems.Count >= 3)
                return "You cannot borrow more than 3 items.";

            borrowedItems.Add(item);
            return $"{item.Title} has been added to {Name}'s list.";
        }

        public string ReturnItem(LibraryItem item)
        {
            if (borrowedItems.Remove(item))
                return $"{item.Title} has been successfully returned.";
            return "That item was not in the list of borrowed items.";
        }

        public IReadOnlyList<LibraryItem> GetBorrowedItems() => borrowedItems;
    }
    #endregion

    #region 图书馆管理类
    class LibraryManager
    {
        private readonly List<LibraryItem> catalog = new();
        private readonly List<Member> members = new();

        public void AddItem(LibraryItem item) => catalog.Add(item);
        public void RegisterMember(Member member) => members.Add(member);

        public void ShowCatalog()
        {
            Console.WriteLine("\n--- Library Catalog ---");
            if (!catalog.Any())
            {
                Console.WriteLine("Catalog is empty.");
                return;
            }
            foreach (var item in catalog)
                Console.WriteLine(item.GetDetails());
        }

        public LibraryItem? FindItemById(int id) => catalog.Find(i => i.Id == id);
        public Member? FindMemberByName(string name) => members.Find(m => m.Name == name);
    }
    #endregion

    #region 主程序
    class Program
    {
        static void Main()
        {
            LibraryManager library = new();

            // 添加若干物品
            library.AddItem(new Novel(1, "The Hobbit", "J.R.R. Tolkien"));
            library.AddItem(new Magazine(2, "National Geographic", 2025));
            library.AddItem(new TextBook(3, "Introduction to Algorithms", "MIT Press"));
            library.AddItem(new Novel(4, "1984", "George Orwell"));

            // 注册会员
            Member alice = new("Alice");
            Member bob = new("Bob");
            library.RegisterMember(alice);
            library.RegisterMember(bob);

            // 显示目录
            library.ShowCatalog();

            // 借书测试
            BorrowAndShow(alice, library.FindItemById(1));
            BorrowAndShow(alice, library.FindItemById(2));
            BorrowAndShow(alice, library.FindItemById(3));
            // 第四次借书应失败
            BorrowAndShow(alice, library.FindItemById(4));

            // 还书测试
            Console.WriteLine(alice.ReturnItem(library.FindItemById(2)!));
            // 再借一次应成功
            BorrowAndShow(alice, library.FindItemById(4));
        }

        static void BorrowAndShow(Member m, LibraryItem? item)
        {
            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }
            Console.WriteLine(m.BorrowItem(item));
        }
    }
    #endregion
}