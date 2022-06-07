# Weather forecast
Develop a console application that allows you to conveniently get the weather for a specific city.
#### Requirements for the functionality of the application:
- ability to get the current weather of a certain city;
- the ability to get a 5-day weather forecast for a specific city;
- selecting a city from a pre-prepared list (`enum`) (5 cities);
- the ability to get the weather for a city that is not in the `enum` (keyboard input);
- weather information is required to display in **the most beautiful form**.

#### Implementation:
The implementation requires using the [Rest API](https://en.wikipedia.org/wiki/Representational_state_transfer) service [Openweathermap](http://openweathermap.org ).    
To communicate with the service, you need to register and get an API key.   
The service returns information in [JSON](https://en.wikipedia.org/wiki/JSON) view.

You need to create classes or structures for this data schema and deserialize using the library [Newtonsoft.Json.NET ](https://www.newtonsoft.com/json ).

#### Requirements for completing the task:
- handle exceptions `(try..catch)`;
- process [status codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) of the service;
- follow the convention of writing C# code.

#### Additional task:

- Develop universal methods for saving and loading `T` to a file along a specific path:
- Develop a universal class implementing the interface `IDictionary<TKey, TValue>`, which acts as a cache for storing previously requested cities and will trigger events when adding and deleting elements.
- Develop an extension for the `object` type, for converting objects into an array of bytes.
- On subsequent downloads of the application, it is required to load the cache from previous sessions of the application.
- For all classes and public methods/fields/properties, it is required to add [XML documenting comments](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/).