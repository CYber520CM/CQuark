﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_LoopForEach : ICQ_Expression {
        public CQ_Expression_LoopForEach (int tbegin, int tend, int lbegin, int lend) {
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
                if(_expressions == null || _expressions.Count == 0)
                    return false;
                foreach(ICQ_Expression expr in _expressions) {
                    if(expr.hasCoroutine)
                        return true;
                }
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            content.DepthAdd();
            CQ_Expression_Define define = _expressions[0] as CQ_Expression_Define;
            define.ComputeValue(content);

            System.Collections.IEnumerable emu = _expressions[1].ComputeValue(content).GetObject() as System.Collections.IEnumerable;

            ICQ_Expression expr_block = _expressions[2] as ICQ_Expression;

            var it = emu.GetEnumerator();
            CQ_Value vrt = CQ_Value.Null;
            while(it.MoveNext()) {
                //content.Set(define.value_name, it.Current);
                CQ_Value val = new CQ_Value();
                val.SetObject(define.value_type, it.Current);
                content.Set(define.value_name, val);


                if(expr_block != null) {
                    if(expr_block is CQ_Expression_Block) {
                        CQ_Value v = expr_block.ComputeValue(content);
                        
						if(v.m_breakBlock == BreakType.Return)
							vrt = v;
						if(v.m_breakBlock == BreakType.Return || v.m_breakBlock == BreakType.Break) 
							break;

                    }
                    else {
                        content.DepthAdd();
                        bool bbreak = false;
                        CQ_Value v = expr_block.ComputeValue(content);

						if(v.m_breakBlock == BreakType.Return)
							vrt = v;
						if(v.m_breakBlock == BreakType.Break || v.m_breakBlock == BreakType.Return) 
							bbreak = true;

                        content.DepthRemove();
                        if(bbreak)
                            break;
                    }
                }
            }

            content.DepthRemove();
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return vrt;

        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            content.DepthAdd();
            CQ_Expression_Define define = _expressions[0] as CQ_Expression_Define;
            define.ComputeValue(content);

            System.Collections.IEnumerable emu = _expressions[1].ComputeValue(content).GetObject() as System.Collections.IEnumerable;

            ICQ_Expression expr_block = _expressions[2] as ICQ_Expression;

            var it = emu.GetEnumerator();

            while(it.MoveNext()) {
                //content.Set(define.value_name, it.Current);
                CQ_Value val = new CQ_Value();
                val.SetObject(define.value_type, it.Current);
                content.Set(define.value_name, val);

                if(expr_block != null) {
                    if(expr_block is CQ_Expression_Block) {
                        if(expr_block.hasCoroutine) {
                            yield return coroutine.StartCoroutine(expr_block.CoroutineCompute(content, coroutine));
                        }
                        else {
                            CQ_Value v = expr_block.ComputeValue(content);

							if(v.m_breakBlock == BreakType.Break || v.m_breakBlock == BreakType.Return)
								break;
                        }
                    }
                    else {
                        content.DepthAdd();
                        bool bbreak = false;
                        if(expr_block.hasCoroutine) {
                            yield return coroutine.StartCoroutine(expr_block.CoroutineCompute(content, coroutine));
                        }
                        else {
                            CQ_Value v = expr_block.ComputeValue(content);

							if(v.m_breakBlock == BreakType.Return || v.m_breakBlock == BreakType.Break)
                                bbreak = true;
                        }
                        content.DepthRemove();
                        if(bbreak)
                            break;
                    }
                }
            }

            content.DepthRemove();
#if CQUARK_DEBUG
			content.OutStack(this);
#endif

        }

        public override string ToString () {
            return "ForEach|";
        }
    }
}