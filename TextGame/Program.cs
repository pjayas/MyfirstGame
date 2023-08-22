using System;
using System.Collections.Generic;
using System.Linq;

internal class Program
{
    private static Character player;

    static void Main(string[] args)
    {
        GameDataSetting();
        DisplayGameIntro();
    }

    static void GameDataSetting()
    {
        // 캐릭터 정보 세팅
        player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

        // 아이템 정보 세팅
        player.Inventory.Add(new Item("검", 2, 0));
        player.Inventory.Add(new Item("방패", 0, 5));
    }

    // 메인 화면
    static void DisplayGameIntro()
    {
        Console.Clear();

        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 전전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int input = CheckValidInput(1, 4);
        switch (input)
        {
            case 1:
                DisplayMyInfo();
                break;

            case 2:
                DisplayInventory();
                break;
            case 3:
                DisplayShop();
                break;
        }
    }

    // 내 정보
    static void DisplayMyInfo()
    {
        Console.Clear();

        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv.{player.Level}");
        Console.WriteLine($"{player.Name}({player.Job})");

        int totalAtkBonus = 0;
        int totalDefBonus = 0;
        Console.WriteLine();
        Console.WriteLine("장착 아이템");
        Console.WriteLine();
        foreach (var equippedItem in player.EquippedItems)
        {
            Console.Write($"{equippedItem.Name,-10}");

            foreach (var inventoryItem in player.Inventory)
            {
                if (inventoryItem.Name == equippedItem.Name)
                {
                    if (inventoryItem.Name == "검")
                    {
                        totalAtkBonus += inventoryItem.Attack;
                        Console.WriteLine($"공격력+{equippedItem.Attack}");
                    }
                    else if (inventoryItem.Name == "방패")
                    {
                        totalDefBonus += inventoryItem.Defense;
                        Console.WriteLine($"방어력+{equippedItem.Defense}");
                    }
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine($"공격력 : {player.Atk + totalAtkBonus}");
        Console.WriteLine($"방어력 : {player.Def + totalDefBonus}");
        Console.WriteLine($"체력 : {player.Hp}");
        Console.WriteLine($"Gold : {player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;
        }
    }

    // 인벤토리
    static void DisplayInventory()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("아이템 목록:");

        for (int i = 0; i < player.Inventory.Count; i++)
        {
            Console.Write($"{i + 1}. {player.Inventory[i].Name,-10}");

            if (player.EquippedItems.Contains(player.Inventory[i]))
            {
                Console.Write("[E] 장착 중");
            }

            Console.WriteLine();
        }

        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, player.Inventory.Count);
        if (input == 0)
        {
            DisplayGameIntro();
        }
        else
        {
            int selectedIndex = input - 1;
            Item selected = player.Inventory[selectedIndex];

            if (player.EquippedItems.Contains(selected))
            {
                player.EquippedItems.Remove(selected);
            }
            else
            {
                player.EquippedItems.Add(selected);
            }

            DisplayInventory();
        }
    }

    // 상점
    static void DisplayShop()
    {
        Console.Clear();
        Console.WriteLine("상점 ");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점 입니다.");
        Console.WriteLine($"현재 골드: {player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("상품 목록:");

        Shop item1 = new Shop("강화된 검", 10, 0, 50);
        Shop item2 = new Shop("철 갑옷", 0, 15, 75);

        Console.WriteLine($"1. {item1.Name} - 공격력 +{item1.Attack}, 가격: {item1.Gold} G");
        Console.WriteLine($"2. {item2.Name} - 방어력 +{item2.Defense}, 가격: {item2.Gold} G");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 2);

        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;
            case 1:
                BuyItem(item1);
                break;
            case 2:
                BuyItem(item2);
                break;
        }
    }

    static void BuyItem(Shop item)
    {
        if (player.Gold >= item.Gold)
        {
            bool alreadyPurchased = player.Inventory.Any(i => i.Name == item.Name);

            if (alreadyPurchased)
            {
                Console.WriteLine($"'{item.Name}'을(를) 이미 구매했습니다.");
            }
            else
            {
                player.Gold -= item.Gold;
                player.Inventory.Add(new Item(item.Name, item.Attack, item.Defense));
                Console.WriteLine($"'{item.Name}'을(를) 구매했습니다!");
            }
        }
        else
        {
            Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
        }

        DisplayShop();
    }

    // 적정성 검사
    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }
}

public class Character
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; }
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    public List<Item> Inventory { get; } = new List<Item>();
    public List<Item> EquippedItems { get; } = new List<Item>();

    public Character(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
    }
}

public class Item
{
    public string Name { get; }
    public int Attack { get; }
    public int Defense { get; }

    public Item(string name, int atk, int def)
    {
        Name = name;
        Attack = atk;
        Defense = def;
    }
}

public class Shop
{
    public string Name { get; }
    public int Attack { get; }
    public int Defense { get; }
    public int Gold { get; }

    public Shop(string name, int atk, int def, int gold)
    {
        Name = name;
        Attack = atk;
        Defense = def;
        Gold = gold;
    }
}
