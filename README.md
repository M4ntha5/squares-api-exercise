# Squares API


### Instructions how to launch
* Download and install postgres15
* Download and install Docker and docker compose
* cd into projects docker folder and run `docker compose -f docker-compose.testing.yml -f docker-compose.yml up`
* Run Squares Api
* Access endpoint in `http://localhost:5044/swagger/index.html`


## Problem Definition
### The bigger picture
The Squares solution is designed to enable our enterprise consumers to identify squares from coordinates and thus make the world a better place.

A user interface allows a user to import a list of points, add and delete a point to an existing list. On top of that the user interface displays a list of squares that our amazing solution identified.

### The task
The Squares UI team is taking care of the front-end side of things, however they need your help for the backend solution.

Create an API that from a given set of points in a 2D plane - enables the consumer to find out sets of points that make squares and how many squares can be drawn. A point is a pair of integer X and Y coordinates. A square is a set of 4 points that when connected make up, well, a square. 

### Example of a list of points that make up a square:
```[(-1;1), (1;1), (1;-1), (-1;-1)]```


## Requirements
### Functional
* I as a user can import a list of points
* I as a user can add a point to an existing list
* I as a user can delete a point from an existing list
* I as a user can retrieve the squares identified

### Non-fuctional
* Include prerequisites and steps to launch in `README`
* The solution code must be in a `git` repository
* The API should be implemented using .NET Core framework (ideally the newest stable version)
* The API must have some sort of persistance layer
* After sending a request the consumer shouldn't wait more than 5 seconds for a response

### Bonus points stuff!
* RESTful API
* Documentation generated from code (hint - `Swagger`)
* Automated tests
* Containerization/deployment (hint - `Docker`)
* Performance considerations
* Measuring SLI's
* Considerations for scalability of the API
* Comments/thoughts on the decisions you made
