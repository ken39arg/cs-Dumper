using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SharpUnit;

using K.Debug;

public class DumperTestCase : TestCase
{
    [UnitTest]
    public void Test ()
    {
        Assert.True(true);

        TestClass putturnA = new TestClass();
        putturnA.pubpubStrA = "pubpubStrA-1";
        putturnA.SetPubpriStrB("pubpriStrB-2");
        putturnA.pubpubIntC = 1231;
        putturnA.SetPripriIntD(4562);
        putturnA.pubpubObjE = new TestClass();
        putturnA.pubpubObjE.pubpubStrA = "pubpubStrA-2";
        putturnA.pubpubObjE.pubpubObjE = putturnA;
        putturnA.pubpubDictF = new Dictionary<int, string>();
        putturnA.pubpubDictF.Add(19, "nineteen");
        putturnA.pubpubDictF.Add(17, "seventeen");
        putturnA.pubpubListG = new List<int>();
        putturnA.pubpubListG.Add(21);
        putturnA.pubpubListG.Add(33);

        Debug.Log(Dumper.Dump(putturnA));
        Debug.Log(Dumper.Dump(putturnA, false));
        Debug.Log(Dumper.Dump(putturnA, 8));
    }
}

public class TestClass
{
    public string pubpubStrA;

    public string pubpriStrB
    {
        get; private set;
    }

    public void SetPubpriStrB(string val)
    {
        pubpriStrB = val;
    }

    public int pubpubIntC;

    private int pripriIntD;

    public void SetPripriIntD(int val)
    {
        pripriIntD = val;
    }

    public TestClass pubpubObjE;

    public IDictionary pubpubDictF;

    public IList pubpubListG;

    public string pubFunc()
    {
        return "str:" + pubpubStrA;
    }

    public TestClass()
    {
    }

}

