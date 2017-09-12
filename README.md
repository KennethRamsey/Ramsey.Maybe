# Ramsey.Maybe
C# Maybe monad


Inspired by haskell's maybe monad, I've created the maybe monad in C#. What makes monads a lot more manageable in haskell is its special syntax for monad, which C# supports a very similar syntax, query expression syntax, which has been overloaded in this library to support Maybe objects.

A Maybe can either be a value or null. To convert something to a maybe, just call the .ToMaybe() extension method. A Maybe isn't guaranteed to have a value, so to get a value you have to use the extension .ValueOrDefault(some_default) to get a value or the specified default.

```c# 
string s = "my string";
Maybe<string> sMaybe = s.ToMaybe(); // has value.
string sVal = sMaybe.ValueOrDefault("default"); // has value "my string";

string s2 = null;
Maybe<string> s2Maybe = s2.ToMaybe(); // is null.
string s2Val = s2Maybe.ValueOrDefault("default"); // has value "Default";
```

Use the query expression syntax to unwrap a maybe and program as if there is a value inside the maybe. If there is no value in the maybe the code will short circuit execution and will return null. The Where operator tests the value and causes the whole expression to return null on failures.

```c#
Maybe<string> ValidateUserInput(string s) =>
    // ensure all data is valid before returning it.
    from sVal in s.ToMaybe()
    where !string.IsNullOrWhiteSpace(sVal)

    let sTrimmed = sVal.Trim()
    
    let reg = Regex.Match(sVal, @".*").Value // fake read function.
    where reg.Length > 5                     // random extra good read condition.

    select sTrimmed;


ValidateUserInput("") // -> null
ValidateUserInput("a") // -> null
ValidateUserInput("12435") // -> "12345"
``` 

Maybes are great for short circuiting on bad inputs. Take this code that gets a persons grandfathers. It quits executing once it determines that it can't find any of the 4 people necessary to gather the grandfathers.

```c#
Maybe<string> getDad(string person) => from pVal in person.ToMaybe()
                                       where !string.IsNullOrWhiteSpace(pVal)
                                       select pVal + "'s dad";

Maybe<string> getMom(string person) => from pVal in persion.ToMaybe()
                                       where !string.IsNullOrEmpty(pVal)
                                       select pVal + "'s mom";

// if these were lookups in a database, we would reduce the calls as much as possible.
// imagine all the ifs you would have to use to code the same logic.
Maybe<(string, string)> GFs(string person) => from dad in getDad(person)
                                              from mom in getMom(person)
                                              from gf1 in getDad(dad)
                                              from gf2 in getDad(mom)
                                              select (gf1, gf2);
```

Finally you can also call of the query syntax methods as extension methods.

```c#
"userInput".ToMaybe()
.WhereNot(string.IsNullOrWhiteSpace)
.Where(x => x.len > 5)
.select(x => x.Trimmed())
.ValueOrDefault("default");
```
