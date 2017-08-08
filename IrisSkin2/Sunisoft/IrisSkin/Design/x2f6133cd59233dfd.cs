namespace Sunisoft.IrisSkin.Design
{
    using Sunisoft.IrisSkin;
    using System;
    using System.CodeDom;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;

    public class x2f6133cd59233dfd : CodeDomSerializer
    {
        public override object Deserialize(IDesignerSerializationManager manager, object codeDomObject)
        {
            CodeDomSerializer serializer = (CodeDomSerializer) manager.GetSerializer(typeof(SkinEngine).BaseType, typeof(CodeDomSerializer));
            return serializer.Deserialize(manager, codeDomObject);
        }

        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            object obj2 = ((CodeDomSerializer) manager.GetSerializer(typeof(SkinEngine).BaseType, typeof(CodeDomSerializer))).Serialize(manager, value);
            bool flag = true;
            if (obj2 is CodeStatementCollection)
            {
                CodeExpression left;
                CodePropertyReferenceExpression expression4;
                CodeStatementCollection statements = (CodeStatementCollection) obj2;
                foreach (CodeStatement statement in statements)
                {
                    if (statement is CodeAssignStatement)
                    {
                        left = ((CodeAssignStatement) statement).Left;
                        if (left is CodePropertyReferenceExpression)
                        {
                            expression4 = (CodePropertyReferenceExpression) left;
                            if ((expression4.PropertyName == "BuiltIn") && (((CodeAssignStatement) statement).Right is CodePrimitiveExpression))
                            {
                                CodePrimitiveExpression right = (CodePrimitiveExpression) ((CodeAssignStatement) statement).Right;
                                if (right.Value is bool)
                                {
                                    flag = (bool) right.Value;
                                }
                            }
                        }
                        else if (left is CodeFieldReferenceExpression)
                        {
                            CodeFieldReferenceExpression expression = (CodeFieldReferenceExpression) ((CodeAssignStatement) statement).Left;
                            if ((expression.FieldName == manager.GetName(value)) && (((CodeAssignStatement) statement).Right is CodeObjectCreateExpression))
                            {
                                CodeObjectCreateExpression expression2 = new CodeObjectCreateExpression(typeof(SkinEngine), new CodeExpression[] { new CodeCastExpression(typeof(Component), new CodeThisReferenceExpression()) });
                                ((CodeAssignStatement) statement).Right = expression2;
                            }
                        }
                    }
                }
                if (flag)
                {
                    return obj2;
                }
                for (int i = statements.Count - 1; i >= 0; i--)
                {
                    CodeStatement statement2 = statements[i];
                    if (statement2 is CodeAssignStatement)
                    {
                        left = ((CodeAssignStatement) statement2).Left;
                        if (left is CodePropertyReferenceExpression)
                        {
                            expression4 = (CodePropertyReferenceExpression) left;
                            if (expression4.PropertyName == "SkinStream")
                            {
                                statements.RemoveAt(i);
                            }
                            else if (expression4.PropertyName == "SkinStreamMain")
                            {
                                statements.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            return obj2;
        }
    }
}

