﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	public class CQ_Expression_Value_Object:ICQ_Expression_Value
	{
		public CQ_Expression_Value_Object(Type type)
		{
			this.type = type;
			this.value_value = null;
		}

		public TypeBridge type
		{
			get;
			private set;
		}

		public object value_value;
		public object value
		{
			get
			{
				return value_value;
			}
		}

		public List<ICQ_Expression> listParam
		{
			get { return null; }
		}
		public int tokenBegin
		{
			get;
			set;
		}
		public int tokenEnd
		{
			get;
			set;
		}
		public int lineBegin
		{
			get;
			set;
		}
		public int lineEnd
		{
			get;
			set;
		}
		public bool hasCoroutine{
			get{
				//				if(listParam == null || listParam.Count == 0)
				//					return false;
				//				foreach(ICQ_Expression expr in listParam){
				//					if(expr.hasCoroutine)
				//						return true;
				//				}
				return false;
			}
		}
		public CQ_Value ComputeValue(CQ_Content content)
		{
			content.InStack(this);
			CQ_Value v = new CQ_Value();

			v.type = this.type;
			v.value = this.value_value;
			content.OutStack(this);
			return v;
		}
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
	}
}