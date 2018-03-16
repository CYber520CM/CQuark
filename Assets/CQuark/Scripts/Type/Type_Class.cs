﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	

	//脚本实例
    public class SInstance
    {
        public CQ_Type type;
        public Dictionary<string, CQ_Content.Value> member = new Dictionary<string, CQ_Content.Value>();//成员
        public Dictionary<string, Dictionary<Type, Delegate>> deles = new Dictionary<string, Dictionary<Type, Delegate>>();
    }
    public class Type_Class : IType
    {
        public Type_Class(string keyword, bool bInterface, string filename)
        {
            this.keyword = keyword;
            this._namespace = "";
            typeBridge = new CQ_Type(keyword, "", filename, bInterface);
            compiled = false;
        }
        public void SetBaseType(IList<IType> types)
        {
            this.types = types;
        }
        IList<IType> types;
        public void EmbDebugToken(IList<Token> tokens)
        {
            ((CQ_Type)typeBridge).EmbDebugToken(tokens);
        }
        public string keyword
        {
            get;
            private set;
        }
        public string _namespace
        {
            get;
            private set;
        }
        public bool compiled
        {
            get;
            set;
        }
        public TypeBridge typeBridge
        {
            get;
            private set;
        }

        public IValue MakeValue(object value)
        {
            CQ_Value_ScriptValue svalue = new CQ_Value_ScriptValue();
            svalue.value_value = value as SInstance;
            svalue.value_type = typeBridge;
            return svalue;
        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
           
			var type = CQuark.AppDomain.GetType(targetType);
            if (this.typeBridge == type||(Type)targetType==typeof(object)) return src;
            if (this.types.Contains(type))
            {
                return src;
            }

            throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
        {
            if (code == LogicToken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left == right.value;
                }
                else
                {
                    return left == right.value;
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left != right.value;
                }
                else
                {
                    return left != right.value;
                }
            }
            throw new NotImplementedException();
        }

        public ICQ_TypeFunction function
        {
            get
            {
                return (CQ_Type)typeBridge as ICQ_TypeFunction;
            }
        }


        public object defaultValue
        {
            get { return null; }
        }



    }
}