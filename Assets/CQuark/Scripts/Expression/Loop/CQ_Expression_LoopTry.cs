﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_LoopTry : ICQ_Expression {
        public CQ_Expression_LoopTry (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
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
            set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions {
            get;
            private set;
        }
        public int tokenBegin {
            get;
            private set;
        }
        public int tokenEnd {
            get;
            set;
        }
        public bool hasCoroutine {
            get {
                //C# 的 try catch 里不允许有协程
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            int oldDepthCount = content.Record();
            try {
                ICQ_Expression expr = _expressions[0];
                if(expr is CQ_Expression_Block) {
                    expr.ComputeValue(content);
                }
                else {
                    content.DepthAdd();
                    expr.ComputeValue(content);
                    content.DepthRemove();
                }

            }
            catch(Exception err) {
                bool bParse = false;
                int i = 1;
                while(i < _expressions.Count) {
                    CQ_Expression_Define def = _expressions[i] as CQ_Expression_Define;
                    if(err.GetType() == (Type)def.value_type || err.GetType().IsSubclassOf((Type)def.value_type)) {
                        content.DepthAdd();
                        CQ_Value errVal = new CQ_Value();
                        errVal.SetObject(def.value_type, err);
						content.DefineAndSet(def.value_name, def.value_type, errVal);

                        _expressions[i + 1].ComputeValue(content);
                        content.DepthRemove();
                        bParse = true;
                        break;
                    }
                    i += 2;
                }
                if(!bParse) {
                    throw err;
                }
            }
            content.Restore(oldDepthCount, this);
            //while((bool)expr_continue.value);

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return CQ_Value.Null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("协程无法包含在try中");
        }

        public override string ToString () {
            return "Try|";
        }
    }
}