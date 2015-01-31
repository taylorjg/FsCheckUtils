using System;
using FsCheck;
using FsCheck.Fluent;

namespace FsCheckUtils
{
    /// <summary>
    /// Extension methods for <see cref="FsCheck.Fluent.Configuration" />.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.MaxNbOfTest"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.MaxNbOfTest"/> property.</param>
        /// <param name="maxTest">The value to use for the <see cref="FsCheck.Fluent.Configuration.MaxNbOfTest"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithMaxTest(this Configuration configuration, int maxTest)
        {
            configuration.MaxNbOfTest = maxTest;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.MaxNbOfFailedTests"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.MaxNbOfFailedTests"/> property.</param>
        /// <param name="maxFail">The value to use for the <see cref="FsCheck.Fluent.Configuration.MaxNbOfFailedTests"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithMaxFail(this Configuration configuration, int maxFail)
        {
            configuration.MaxNbOfFailedTests = maxFail;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.Name"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.Name"/> property.</param>
        /// <param name="name">The value to use for the <see cref="FsCheck.Fluent.Configuration.Name"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithName(this Configuration configuration, string name)
        {
            configuration.Name = name;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.StartSize"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.StartSize"/> property.</param>
        /// <param name="startSize">The value to use for the <see cref="FsCheck.Fluent.Configuration.StartSize"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithStartSize(this Configuration configuration, int startSize)
        {
            configuration.StartSize = startSize;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.EndSize"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.EndSize"/> property.</param>
        /// <param name="endSize">The value to use for the <see cref="FsCheck.Fluent.Configuration.EndSize"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithEndSize(this Configuration configuration, int endSize)
        {
            configuration.EndSize = endSize;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.Every"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.Every"/> property.</param>
        /// <param name="every">The value to use for the <see cref="FsCheck.Fluent.Configuration.Every"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithEvery(this Configuration configuration, Func<int, object[], string> every)
        {
            configuration.Every = every;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.EveryShrink"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.EveryShrink"/> property.</param>
        /// <param name="everyShrink">The value to use for the <see cref="FsCheck.Fluent.Configuration.EveryShrink"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithEveryShrink(this Configuration configuration, Func<object[], string> everyShrink)
        {
            configuration.EveryShrink = everyShrink;
            return configuration;
        }

        /// <summary>
        /// Sets the <see cref="FsCheck.Fluent.Configuration.Runner"/> property of a
        /// <see cref="FsCheck.Fluent.Configuration"/> object.
        /// </summary>
        /// <param name="configuration">The <see cref="FsCheck.Fluent.Configuration"/> object on which
        /// to set the <see cref="FsCheck.Fluent.Configuration.Runner"/> property.</param>
        /// <param name="runner">The value to use for the <see cref="FsCheck.Fluent.Configuration.Runner"/> property.</param>
        /// <returns>The <paramref name="configuration" /> object to allow chaining.</returns>
        public static Configuration WithRunner(this Configuration configuration, IRunner runner)
        {
            configuration.Runner = runner;
            return configuration;
        }
    }
}
