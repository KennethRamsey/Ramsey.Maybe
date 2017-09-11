# Ramsey.Maybe
C# Maybe monad


Inspired by haskell by it's maybe monad, I've created the maybe monad in C#. What makes monads a lot more manageable in haskell is its special syntax for monad, which C# supports an very similar syntax, query expression syntax, which has been incorporated into this library.

To convert something to a maybe, just call the .ToMaybe() extension method. A Maybe might not have a value, so to get a value you have to use the extension .ValueOrDefault(some_default) to get a value or the specified default. 

Use the query expression syntax to unwrap a maybe and program as if there is a value inside the maybe. If there is no value in the maybe the code will short circuit execution and will just return null. 

```c#
from val in userInput.ToMaybe()
where !string.IsNullOrWhitespace(val)
let reg = Regex.Match(sVal, @".*").Value // fake read function.
where reg.Length > 5					 // random extra good read condition.
select updated;
``` 

Maybes are great for short circuiting on bad inputs. Take this code that gets a persons grandfathers. It quits executing once it determines that it can't find any of the 4 people necessary to gather the grandfathers.

```c#
Maybe<string> getDad(string person) => from pVal in person.ToMaybe()
                                       where !string.IsNullOrWhiteSpace(pVal) // quickly apply input filtering.
                                       select pVal + "'s dad";

Maybe<string> getMom(string person) => from pVal in persion.ToMaybe()
                                       where !string.IsNullOrEmpty(pVal)
                                       select pVal + "'s mom";

// imagine these were lookups in a database, and all the ifs you would have to use to code the same login.
Maybe<(string, string)> GFs(string person) => from dad in getDad(person)
                                              from mom in getMom(person)
                                              from gf1 in getDad(dad)
                                              from gf2 in getDad(mom)
                                              select (gf1, gf2);
```
