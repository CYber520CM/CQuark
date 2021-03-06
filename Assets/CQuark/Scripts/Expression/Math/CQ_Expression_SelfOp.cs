﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_SelfOp : ICQ_Expression {
        public CQ_Expression_SelfOp (int tbegin, int tend, int lbegin, int lend) {
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            private set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions {
            get {
                return null;
            }
        }
        public int tokenBegin {
            get;
            private set;
        }
        public int tokenEnd {
            get;
            private set;
        }
        public bool hasCoroutine {
            get {
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            CQ_Value v = content.Get(value_name);
            IType type = CQuark.AppDomain.GetITypeByCQValue(v);
            CQ_Value retVal = type.Math2Value(mathop, v, CQ_Value.One);
            v.UsingValue(retVal);
			content.Set(value_name, v);

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return v;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("SelfOp不支持套用协程");
        }


        public string value_name;
        public char mathop;

        public override string ToString () {
            return "MathSelfOp|" + value_name + mathop;
        }
    }
}