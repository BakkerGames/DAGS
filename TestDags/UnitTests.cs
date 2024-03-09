using DAGS;
using System.Text;

namespace TestDags;

public class UnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestPassing()
    {
        Assert.Pass();
    }

    [Test]
    public void TestGet()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        data.Add(key, value);
        StringBuilder result = new();
        dags.RunScript($"@get({key})", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestSet()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        StringBuilder result = new();
        dags.RunScript($"@set({key},{value})", result);
        dags.RunScript($"@get({key})", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestSetArray()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        StringBuilder result = new();
        dags.RunScript($"@setarray({key},2,3,{value})", result);
        dags.RunScript($"@getarray({key},2,3)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestSetArray_Null()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "";
        StringBuilder result = new();
        dags.RunScript($"@setarray({key},2,3,{value})", result);
        dags.RunScript($"@getarray({key},2,3)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestClearArray()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "";
        StringBuilder result = new();
        dags.RunScript($"@setarray({key},2,3,{value})", result);
        dags.RunScript($"@cleararray({key})", result);
        dags.RunScript($"@getarray({key},2,3)", result);
        Assert.That(result.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void TestSetList()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        StringBuilder result = new();
        dags.RunScript($"@setlist({key},1,{value})", result);
        dags.RunScript($"@getlist({key},1)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestSetList_Null()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "";
        StringBuilder result = new();
        dags.RunScript($"@setlist({key},1,{value})", result);
        dags.RunScript($"@getlist({key},1)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestInsertAtList()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        StringBuilder result = new();
        dags.RunScript($"@addlist({key},0)", result);
        dags.RunScript($"@addlist({key},1)", result);
        dags.RunScript($"@addlist({key},2)", result);
        dags.RunScript($"@addlist({key},3)", result);
        dags.RunScript($"@insertatlist({key},1,{value})", result);
        dags.RunScript($"@getlist({key},1)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
        result.Clear();
        dags.RunScript($"@getlist({key},4)", result);
        Assert.That(result.ToString(), Is.EqualTo("3"));
    }

    [Test]
    public void TestRemoveAtList()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        var key = "abc";
        var value = "123";
        StringBuilder result = new();
        dags.RunScript($"@setlist({key},3,{value})", result);
        dags.RunScript($"@removeatlist({key},0)", result);
        dags.RunScript($"@getlist({key},2)", result);
        Assert.That(result.ToString(), Is.EqualTo(value));
    }

    [Test]
    public void TestFunction()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(\"@boo\",\"@write(eek!)\")", result);
        dags.RunScript("@boo", result);
        Assert.That(result.ToString(), Is.EqualTo("eek!"));
    }

    [Test]
    public void TestFunctionParameters()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(\"@boo(x)\",\"@write($x)\")", result);
        dags.RunScript("@boo(eek!)", result);
        Assert.That(result.ToString(), Is.EqualTo("eek!"));
    }

    [Test]
    public void TestValidateSucceed()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        bool value = dags.ValidateScript("@set(key,value)", result);
        Assert.That(value, Is.EqualTo(true));
    }

    [Test]
    public void TestValidateFail()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        bool value = dags.ValidateScript("@blah(key)", result);
        Assert.That(value, Is.EqualTo(false));
    }

    [Test]
    public void Test_Abs()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@abs(1))", result);
        Assert.That(result.ToString(), Is.EqualTo("1"));
        result.Clear();
        dags.RunScript("@write(@abs(-1))", result);
        Assert.That(result.ToString(), Is.EqualTo("1"));
    }

    [Test]
    public void Test_Add()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@add(1,3))", result);
        Assert.That(result.ToString(), Is.EqualTo("4"));
    }

    [Test]
    public void Test_AddTo()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,12) @addto(value,7) @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("19"));
    }

    [Test]
    public void Test_Comment()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@comment(\"this is a comment\")", result);
        Assert.That(result.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void Test_Concat()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@concat(abc,def,123))", result);
        Assert.That(result.ToString(), Is.EqualTo("abcdef123"));
    }

    [Test]
    public void Test_Div()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@div(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void Test_DivTo()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,12) @divto(value,3) @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("4"));
    }

    [Test]
    public void Test_EQ()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@eq(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@eq(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_Exec()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@exec(\"@set(value,23)\") @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("23"));
    }

    [Test]
    public void Test_False()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@false(\"\"))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@false(0))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@false(1))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@false(abc))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_FalseData()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(test.value,\"\") @write(@falsedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@set(test.value,false) @write(@falsedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@set(test.value,1) @write(@falsedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@set(test.value,abc) @write(@falsedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_For()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@for(x,1,3) @write($x) @endfor", result);
        Assert.That(result.ToString(), Is.EqualTo("123"));
    }

    [Test]
    public void Test_ForEachKey()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(value.1,100) @set(value.2,200)", result);
        result.Clear();
        dags.RunScript("@foreachkey(x,\"value.\") @write($x) @endforeachkey", result);
        Assert.That(result.ToString(), Is.EqualTo("12"));
        result.Clear();
        dags.RunScript("@foreachkey(x,\"value.\") @get(value.$x) @endforeachkey", result);
        Assert.That(result.ToString(), Is.EqualTo("100200"));
    }

    [Test]
    public void Test_ForEachList()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(value,\"10,20,30\")", result);
        result.Clear();
        dags.RunScript("@foreachlist(x,value) @write($x) @endforeachlist", result);
        Assert.That(result.ToString(), Is.EqualTo("102030"));
    }

    [Test]
    public void Test_Format()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@format(\"{0}-{1}-{2}\",1,2,3))", result);
        Assert.That(result.ToString(), Is.EqualTo("1-2-3"));
        result.Clear();
        dags.RunScript("@write(@format(\"{2}-{1}-{0}\",1,2,3))", result);
        Assert.That(result.ToString(), Is.EqualTo("3-2-1"));
        result.Clear();
        dags.RunScript("@write(@format(\"{0}-{1}-{2}\",1,2))", result);
        Assert.That(result.ToString(), Is.EqualTo("1-2-{2}"));
    }

    [Test]
    public void Test_GE()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@ge(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@ge(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@ge(1,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_GetInChannel()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.InChannel.Enqueue("abc");
        dags.InChannel.Enqueue("123");
        result.Clear();
        dags.RunScript("@write(@getinchannel)", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
        result.Clear();
        dags.RunScript("@write(@getinchannel)", result);
        Assert.That(result.ToString(), Is.EqualTo("123"));
        result.Clear();
        dags.RunScript("@write(@getinchannel)", result);
        Assert.That(result.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void Test_GetValue()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(v1,\"@get(v2)\") @set(v2,123)", result);
        result.Clear();
        dags.RunScript("@get(v1)", result);
        Assert.That(result.ToString(), Is.EqualTo("@get(v2)"));
        result.Clear();
        dags.RunScript("@write(@getvalue(v1))", result);
        Assert.That(result.ToString(), Is.EqualTo("123"));
    }

    [Test]
    public void Test_GT()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@gt(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@gt(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@gt(1,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_If()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@if true @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
        result.Clear();
        dags.RunScript("@if false @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("def"));
        result.Clear();
        dags.RunScript("@if true @or false @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
        result.Clear();
        dags.RunScript("@if true @and false @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("def"));
        result.Clear();
        dags.RunScript("@if null @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("def"));
    }

    [Test]
    public void Test_IsBool()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@isbool(0))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@isbool(1))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@isbool(notboolean))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_IsBoolData()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(test.value,0) @write(@isbooldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@set(test.value,1) @write(@isbooldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@set(test.value,notboolean) @write(@isbooldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_IsNull()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@isnull(null))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@isnull(abc))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@isnull(@get(value)))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_IsNullData()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(test.value,null) @write(@isnulldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@set(test.value,abc) @write(@isnulldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@set(test.value,\"\") @write(@isnulldata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_IsScript()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@isscript(abc))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@isscript(\"@get(value)\"))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_IsScriptData()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(test.value,abc) @write(@isscriptdata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@set(test.value,\"@get(value)\") @write(@isscriptdata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_LE()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@le(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@le(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@le(1,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_Lower()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@lower(ABC)", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
        result.Clear();
        dags.RunScript("@lower(DEF)", result);
        Assert.That(result.ToString(), Is.EqualTo("def"));
    }

    [Test]
    public void Test_LT()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@lt(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@lt(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@lt(1,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_Mod()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@mod(13,4))", result);
        Assert.That(result.ToString(), Is.EqualTo("1"));
        result.Clear();
        dags.RunScript("@write(@mod(12,4))", result);
        Assert.That(result.ToString(), Is.EqualTo("0"));
    }

    [Test]
    public void Test_ModTo()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,13) @modto(value,4) @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("1"));
    }

    [Test]
    public void Test_Msg()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,abcdef) @msg(value)", result);
        Assert.That(result.ToString(), Is.EqualTo("abcdef\\n"));
    }

    [Test]
    public void Test_Mul()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@mul(3,4))", result);
        Assert.That(result.ToString(), Is.EqualTo("12"));
    }


    [Test]
    public void Test_MulTo()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,3) @multo(value,4) @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("12"));
    }

    [Test]
    public void Test_NE()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@ne(42,6))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
        result.Clear();
        dags.RunScript("@write(@ne(42,42))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void Test_NL()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@nl", result);
        Assert.That(result.ToString(), Is.EqualTo("\\n"));
    }

    [Test]
    public void Test_Not()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@if @not false @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void Test_Or()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@if true @or false @then @write(abc) @else @write(def) @endif", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void Test_Rand()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@rand(30)", result);
        Assert.That(result.ToString() == "true" || result.ToString() == "false");
    }

    [Test]
    public void Test_Replace()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@replace(abcdef,d,x))", result);
        Assert.That(result.ToString(), Is.EqualTo("abcxef"));
    }

    [Test]
    public void Test_Rnd()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(value,@rnd(20))", result);
        result.Clear();
        dags.RunScript("@get(value)", result);
        var r1 = int.Parse(result.ToString());
        Assert.That(r1 >= 0 && r1 < 20);
    }

    [Test]
    public void Test_Script()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        dags.RunScript("@set(script1,\"@write(abc)\")", result);
        result.Clear();
        dags.RunScript("@script(script1)", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void Test_SetOutChannel()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@setoutchannel(abc)", result);
        var value = dags.OutChannel.Dequeue() ?? "";
        Assert.That(value, Is.EqualTo("abc"));
    }

    [Test]
    public void Test_Sub()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@sub(1,3))", result);
        Assert.That(result.ToString(), Is.EqualTo("-2"));
    }

    [Test]
    public void Test_Substring()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@substring(abcdef,1,4))", result);
        Assert.That(result.ToString(), Is.EqualTo("bcde"));
    }

    [Test]
    public void Test_SubTo()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,12) @subto(value,7) @write(@get(value))", result);
        Assert.That(result.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void Test_Swap()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value1,abc) @set(value2,def) @swap(value1,value2) @write(@get(value1),@get(value2))", result);
        Assert.That(result.ToString(), Is.EqualTo("defabc"));
    }

    [Test]
    public void Test_Trim()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(value,\"   abc   \") @write(@trim(@get(value)))", result);
        Assert.That(result.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void Test_True()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@true(0))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@write(@true(1))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_TrueData()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@set(test.value,0) @write(@truedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("false"));
        result.Clear();
        dags.RunScript("@set(test.value,1) @write(@truedata(test.value))", result);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void Test_Upper()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        result.Clear();
        dags.RunScript("@write(@upper(abc))", result);
        Assert.That(result.ToString(), Is.EqualTo("ABC"));
    }

    [Test]
    public void Test_Help()
    {
        var helpText = Dags.Help();
        Assert.That(helpText, !Is.EqualTo(null));
    }

    [Test]
    public void Test_PrettyScript()
    {
        var script = "@if @eq(@get(value),0) @then @write(\"zero\") @else @write(\"not zero\") @endif";
        var expected = "@if @eq(@get(value),0) @then\r\n\t@write(\"zero\")\r\n@else\r\n\t@write(\"not zero\")\r\n@endif";
        var actual = Dags.PrettyScript(script);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_PrettyScript_Same()
    {
        var script = "@write(\"hello \\\"wonderful\\\" world.\")";
        var actual = Dags.PrettyScript(script);
        Assert.That(actual, Is.EqualTo(script));
    }

    [Test]
    public void Test_IfThenNoStatements()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        var script = "@if @eq(1,1) @then @endif";
        dags.RunScript(script, result);
        Assert.That(result.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void Test_IfThenElseNoStatements()
    {
        Dictionary<string, string> data = [];
        Dags dags = new(data);
        StringBuilder result = new();
        var script = "@if @eq(1,2) @then @write(abc) @else @endif";
        dags.RunScript(script, result);
        Assert.That(result.ToString(), Is.EqualTo(""));
    }
}
