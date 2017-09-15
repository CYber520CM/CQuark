﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{

    public class CLS_Expression_NegativeLogic : ICLS_Expression
    {
        public CLS_Expression_NegativeLogic(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            private set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            private set;
        }
		public bool hasCoroutine{
			get{
				if(listParam == null || listParam.Count == 0)
					return false;
				foreach(ICLS_Expression expr in listParam){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);

            CLS_Content.Value r = listParam[0].ComputeValue(content);


            CLS_Content.Value r2 = new CLS_Content.Value();
            r2.type = r.type;
            r2.breakBlock = r.breakBlock;
            r2.value = !(bool)r.value;
            content.OutStack(this);

            return r2;
        }

		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}


        public override string ToString()
        {
            return "(!)|" ;
        }
    }
}