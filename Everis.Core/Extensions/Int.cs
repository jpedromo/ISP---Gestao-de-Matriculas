using System;

namespace Everis.Core.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Executes the specified <paramref name="action"/> the specified number of times, passing in the iterator value each time.
        /// </summary>
        /// <param name="value">the number of times to execute the action.</param>
        /// <param name="action">The delegate action to be executed, passing in the iterator value each time.</param>
        public static void Time(this int value, Action<int> action)
        {
            for (int i = 0; i < value; i++)
            {
                action(i);
            }
        }

        /// <summary>
        ///  Iterates from the this instance value through the specified <paramref name="stopAt"/> and calls the specified <paramref name="action"/> for each passing in the iterator.
        /// </summary>
        /// <param name="value">The starting number</param>
        /// <param name="stopAt">The upper <see cref="int"/> value for the iterator.</param>
        /// <param name="action">The delegate action to be executed, passing in the iterator value each time.</param>
        public static void UpTo(this int value, int stopAt, Action<int> action)
        {
            if (value > stopAt)
            {
                throw new ArgumentOutOfRangeException("The stopAt parameter must be greater than {0}!".FormatWith(value));
            } 

            for (int i = value; i <= stopAt; i++)
            {
                action(i);
            }
        }
    }
}
