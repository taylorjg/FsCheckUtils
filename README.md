
## Provides some convenience extension methods

### Config

<code>FsCheck.Config</code> is an immutable class. The following extension methods create a new
<code>FsCheck.Config</code> object with an updated value.

* Config.WithMaxSize
* Config.WithMaxFail
* Config.WithReplay
* Config.WithNoReplay
* Config.WithName
* Config.WithStartSize
* Config.WithEndSize
* Config.WithEvery
* Config.WithEveryShrink
* Config.WithArbitrary
* Config.WithRunner

### Configuration

<code>FsCheck.Fluent.Configuration</code> is a mutable version of <code>FsCheck.Config</code>.
The following method makes it easy to create an instance of <code>FsCheck.Fluent.Configuration</code> from an instance of <code>FsCheck.Config</code>.

* Config.ToConfiguration

As mentioned above, <code>FsCheck.Fluent.Configuration</code> is mutable.
However, the following extension methods allow multiple properties to be set in a chained
style.

* Configuration.WithMaxTest
* Configuration.WithMaxFail
* Configuration.WithName
* Configuration.WithStartSize
* Configuration.WithEndSize
* Configuration.WithEvery
* Configuration.WithEveryShrink
* Configuration.WithRunner

## Provides wrappers around FsCheck operators

* PropExtensions.And (.&.)
* PropExtensions.AndAll (.&.)
* PropExtensions.Or (.|.)
* PropExtensions.OrAll (.|.)
* PropExtensions.Label (|@)
* PropExtensions.Implies (==>)

## Provides functionality that is in ScalaCheck but not in FsCheck

* GenExtensions.pick
* GenExtensions.someOf

## TODO

* NumChar
* AlphaUpperChar
* AlphaLowerChar
* AlphaChar
* AlphaNumChar

* Identifier
* AlphaStr
* NumStr

* Uuid
   * See http://en.wikipedia.org/wiki/Universally_unique_identifier#Version_4_.28random.29

* zip (arity 2 to 9)
    * Generates Tuple2 from g1/g2, Tuple3 from g1/g2/g3, etc

* retryUntil
* containerOf / Buildable
