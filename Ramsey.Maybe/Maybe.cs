using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.Maybe
{

    /// <summary>
    /// Wrapper used to help program over objects that could or could not exist. Null is used for the nothing maybe value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Maybe<T> { }


    // you really don't need a nothing class, in c# at least.
    // public class Nothing<T> : Maybe<T>
    //{
    //	public override string ToString() => "Nothing";
    //}

    
    /// <summary>
    /// Do not use this class. Only use the maybe extensions methods.
    /// </summary>
    public class Just<T> : Maybe<T>
    {
        public T Value { get; private set; }

        /// <summary>
        /// Especially don't use this constructor. ONLY use the extension .ToMaybe() to create maybe objects.
        /// </summary>
        public Just(T value)
        {
            // the extention checks for null, so we don't need to check here.
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }


    public static class MaybeExtensions
    {
        /// <summary>
        /// Returns the given object wrapped in a maybe, or a 'nothing' if the object was null.
        /// </summary>        
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            if (value == null)
                return null; // well look at that! this won't let us put null inside a maybe!

            return new Just<T>(value);
        }


        /// <summary>
        /// Returns the value inside the maybe, or the given default.
        /// </summary>        
        /// <param name="maybe">Value to try and get.</param>
        /// <param name="def">Default if object is null.</param>
        public static T ValueOrDefault<T>(this Maybe<T> maybe, T def = default(T)) =>
            maybe is Just<T> val
            ? val.Value
            : def;


        /// <summary>
        /// Check if a maybe is nothing.
        /// </summary>
        public static bool IsNothing<T>(this Maybe<T> maybe) => maybe == null;


        // this is more for reference, as i like to inline the call to bind for performance, as this will happen alot.
        //public static Maybe<B> Bind<A, B>(this Maybe<A> a, Func<A, Maybe<B>> func) =>
        //    a is Just<A> justa
        //    ? func(justa.Value)
        //    : null;


        /// <summary>
        /// Does the transformation if the maybe has a value, otherwise returns null.
        /// </summary>
        public static Maybe<B> Select<A, B>(this Maybe<A> a, Func<A, B> select)
        {
            return a is Just<A> justa
                ? select(justa.Value).ToMaybe()
                : null;
        }


        /// <summary>
        /// Returns the object if it passes the given filter, otherwise returns null.
        /// </summary>        
        public static Maybe<A> Where<A>(this Maybe<A> a, Func<A, bool> pred)
        {         
            return
                a is Just<A> val && pred(val.Value)
                ? a
                : null;
        }


        /// <summary>
        /// Returns the object if it doesn't satisfy the filter, otherwise returns null.
        /// </summary>
        public static Maybe<A> WhereNot<A>(this Maybe<A> a, Func<A, bool> pred)
        {
            return
                a is Just<A> val && !pred(val.Value)
                ? a
                : null;
        }


        /// <summary>
        /// Allows you to write longer queries over maybes in query syntax.
        /// </summary>
        public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> a, Func<A, Maybe<B>> func, Func<A, B, C> select)
        {
            if (a is Just<A> justa)
            {
                if (func(justa.Value) is Just<B> justb)
                    return select(justa.Value, justb.Value).ToMaybe();
                else
                    return null;
            }
            else
                return null;
        }
    }
}
