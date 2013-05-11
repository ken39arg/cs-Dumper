
Dumper for Unity (c#)
===================

dump object.

public fields and public property


Ex
------------------

c# code

    using UnityEngine;
    using System;
    
    using K.Debug;
    
    // main
    {
        TestClass putturnA = new TestClass();
        putturnA.pubpubStrA = "pubpubStrA-1";
        putturnA.SetPubpriStrB("pubpriStrB-2");
        putturnA.pubpubIntC = 1231;
        putturnA.SetPripriIntD(4562);
        putturnA.pubpubObjE = new TestClass();
        putturnA.pubpubObjE.pubpubStrA = "pubpubStrA-2";
        putturnA.pubpubDictF = new Dictionary<int, string>();
        putturnA.pubpubDictF.Add(19, "nineteen");
        putturnA.pubpubDictF.Add(17, "seventeen");
        putturnA.pubpubListG = new List<int>();
        putturnA.pubpubListG.Add(21);
        putturnA.pubpubListG.Add(33);
        
        Debug.Log(Dumper.Dump(putturnA));
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
    
        public TestClass(){}
    
    }
    class Foo
    {
        void Metod(Bar bar)
        {
            Debug.Log(Dumper.Dump(bar);
        }
    }


log


    TestClass (
        (String) pubpubStrA = "pubpubStrA-1";
        (Int32) pubpubIntC = 1231;
        (TestClass) pubpubObjE = TestClass (
            (String) pubpubStrA = "pubpubStrA-2";
            (Int32) pubpubIntC = 0;
            (TestClass) pubpubObjE = Null;
            (IDictionary) pubpubDictF = Null;
            (IList) pubpubListG = Null;
            (String) pubpriStrB = Null;
        );
        (IDictionary) pubpubDictF = Dictionary`2<Int32,String> {
            19 : "nineteen",
            17 : "seventeen",
        };
        (IList) pubpubListG = List`1<Int32> [
            21,
            33,
        ];
        (String) pubpriStrB = "pubpriStrB-2";
    )
