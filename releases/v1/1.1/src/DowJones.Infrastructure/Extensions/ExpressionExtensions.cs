// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ExpressionExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq.Expressions;

namespace DowJones.Utilities.Extensions
{
    public static class ExpressionExtensions
    {
        public static string MemberWithoutInstance(this LambdaExpression expression)
        {
            var memberExpression = expression.ToMemberExpression();

            if (memberExpression == null)
            {
                return null;
            }

            if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var innerMemberExpression = (MemberExpression)memberExpression.Expression;

                while (innerMemberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    innerMemberExpression = (MemberExpression)innerMemberExpression.Expression;
                }

                var parameterExpression = (ParameterExpression)innerMemberExpression.Expression;

                return memberExpression.ToString().Substring(parameterExpression.Name.Length + 1);
            }

            return memberExpression.Member.Name;
        }

        public static bool IsBindable(this LambdaExpression expression)
        {
            switch (expression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Parameter:
                    return true;
            }

            return false;
        }

        public static MemberExpression ToMemberExpression(this LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;

                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            return memberExpression;
        }
    }
}
