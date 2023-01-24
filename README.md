# DurableFunctionsDemo

Durable functions is an extention of Azure functions that helps to implement stateful functions in serverles computing. Using the Azure functions programming model, it allows us to define stateful workflows by writing ochestrator functions and stateful entities by writing activity functions.

Ochestrator Function - This is the function where the workflow is defined that allows to set up the activities need to carry out. 

Activity Function - A basic unit of work in a durable ochestration that are ochestrated the work processes 

Client Function - The entry point of creating a durable function ochestration. When it triggered it creates a new instance of an ochestration. 
