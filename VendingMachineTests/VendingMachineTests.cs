using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;


[TestFixture]
public class VendingMachineTests
{
    private VendingMachine CreateMachine()
    {
        return new VendingMachine();
    }

    [Test]
    public void Insert_ValidCoin_UpdatesCurrentAmount()
    {
        var machine = CreateMachine();
        InvokePrivateMethod(machine, "InsertCoin", "quarter");

        string display = GetPrivateField<string>(machine, "lastMessage");

        Assert.AreEqual("$0.25", display);
    }

    [Test]
    public void Insert_InvalidCoin_AddsToCoinReturn()
    {
        var machine = CreateMachine();
        InvokePrivateMethod(machine, "InsertCoin", "penny");

        var coinReturn = GetPrivateField<List<string>>(machine, "coinReturn");

        Assert.Contains("penny", coinReturn);
    }

    [Test]
    public void SelectProduct_InsufficientAmount_ShowsPrice()
    {
        var machine = CreateMachine();
        InvokePrivateMethod(machine, "SelectProduct", "chips");

        string display = GetPrivateField<string>(machine, "lastMessage");

        Assert.AreEqual("PRICE $0.50", display);
    }

    [Test]
    public void SelectProduct_SufficientAmount_DeductsAndThanks()
    {
        var machine = CreateMachine();
        InvokePrivateMethod(machine, "InsertCoin", "quarter");
        InvokePrivateMethod(machine, "InsertCoin", "quarter");

        InvokePrivateMethod(machine, "SelectProduct", "chips");

        string display = GetPrivateField<string>(machine, "lastMessage");

        Assert.AreEqual("THANK YOU", display);
    }

    [Test]
    public void GetDisplay_ShowsInsertCoinWhenEmpty()
    {
        var machine = CreateMachine();
        string display = InvokePrivateMethod<string>(machine, "GetDisplay");

        Assert.AreEqual("INSERT COIN", display);
    }

    [Test]
    public void GetDisplay_UpdatesAfterThankYou()
    {
        var machine = CreateMachine();
        InvokePrivateMethod(machine, "InsertCoin", "quarter");
        InvokePrivateMethod(machine, "InsertCoin", "quarter");
        InvokePrivateMethod(machine, "SelectProduct", "chips");

        string firstDisplay = InvokePrivateMethod<string>(machine, "GetDisplay");
        Assert.AreEqual("THANK YOU", firstDisplay);

        string secondDisplay = InvokePrivateMethod<string>(machine, "GetDisplay");
        Assert.AreEqual("INSERT COIN", secondDisplay);
    }


    private T InvokePrivateMethod<T>(object obj, string methodName, params object[] parameters)
    {
        var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)method.Invoke(obj, parameters);
    }

    private void InvokePrivateMethod(object obj, string methodName, params object[] parameters)
    {
        var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(obj, parameters);
    }

    private T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)field.GetValue(obj);
    }
}
