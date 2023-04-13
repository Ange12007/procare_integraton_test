// <copyright file="NotImplementedException.cs" company="Procare Software, LLC">
// Copyright © 2021-2023 Procare Software, LLC. All rights reserved.
// </copyright>

namespace Procare.Address.IntegrationTests
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
#pragma warning disable SA1600 // Elements should be documented
    public class NotImplementedException : Exception
#pragma warning restore SA1600 // Elements should be documented
    {
#pragma warning disable SA1614 // Element parameter documentation should have text
        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        /// <param name="e"></param>
        public NotImplementedException(Exception e)
#pragma warning restore SA1614 // Element parameter documentation should have text
        {
            if (e is not null)
            {
            Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        public NotImplementedException()
        {
        }

#pragma warning disable SA1614 // Element parameter documentation should have text

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public NotImplementedException(string message)
#pragma warning restore SA1614 // Element parameter documentation should have text
            : base(message)
        {
        }

#pragma warning disable SA1614

        // Element parameter documentation should have text

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NotImplementedException(string message, Exception innerException)
#pragma warning restore SA1614 // Element parameter documentation should have text
            : base(message, innerException)
        {
        }

        private string GetDebuggerDisplay()
        {
            return this.ToString();
        }
    }
}
