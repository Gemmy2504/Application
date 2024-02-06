API Documentation: Vending Machine
Introduction
Welcome to the documentation for the Vending Machine API. This API provides endpoints for managing users, products, and vending machine transactions.

////////////////////

first of all i wanna let you know i didn't use data base so i just use lists and added the product/user to it
i used some hard coded data to test
so some functions will not run but i submitted before the dead line,
thanks in advanced  

//////////////////////

Resources

	User
GET /api/users
Description: Retrieve the list of all users.
Authentication: Not required.
Response:
Status Code: 200 OK
Body: Array of User objects.

GET /api/users/{id}
Description: Retrieve a specific user by ID.
Authentication: Not required.
Response:
Status Code: 200 OK
Body: User object.
Status Code: 404 Not Found if the user is not found.

POST /api/users
Description: Register a new user.
Authentication: Not required.
Request Body:
User object with the following fields: Username, Password, Deposit, Role.
Response:
Status Code: 201 Created
Body: User object with assigned UserId.
Status Code: 400 Bad Request if the username is already taken.

PUT /api/users/{id}
Description: Update user information.
Authentication: Requires seller role.
Request Body:
Updated User object with fields to modify.
Response:
Status Code: 200 OK
Body: Updated User object.
Status Code: 404 Not Found if the user is not found.

DELETE /api/users/{id}
Description: Delete a user.
Authentication: Requires seller role.
Response:
Status Code: 204 No Content
Status Code: 404 Not Found if the user is not found.
Product

GET /api/products
Description: Retrieve the list of all products.
Authentication: Not required.
Response:
Status Code: 200 OK
Body: Array of Product objects.

GET /api/products/{id}
Description: Retrieve a specific product by ID.
Authentication: Not required.
Response:
Status Code: 200 OK
Body: Product object.
Status Code: 404 Not Found if the product is not found.

POST /api/products
Description: Add a new product.
Authentication: Requires seller role.
Request Body:
Product object with the following fields: ProductName, AmountAvailable, Cost, SellerId.
Response:
Status Code: 201 Created
Body: Product object with assigned ProductId.
Status Code: 400 Bad Request if the seller is not found.

PUT /api/products/{id}
Description: Update product information.
Authentication: Requires seller role.
Request Body:
Updated Product object with fields to modify.
Response:
Status Code: 200 OK
Body: Updated Product object.
Status Code: 404 Not Found if the product is not found.

DELETE /api/products/{id}
Description: Delete a product.
Authentication: Requires seller role.
Response:
Status Code: 204 No Content
Status Code: 404 Not Found if the product is not found.
Buyer Operations

POST /api/users/reset
Description: Reset a buyer's deposit to zero.
Authentication: Requires buyer role.
Response:
Status Code: 200 OK
Body: Success message.
Status Code: 400 Bad Request if the buyer is not found.

POST /api/users/deposit
Description: Deposit coins into a buyer's account.
Authentication: Requires buyer role.
Request Body:
JSON object with Amount representing the deposited cents.
Response:
Status Code: 200 OK
Body: Success message with the updated deposit amount.
Status Code: 400 Bad Request if the buyer is not found.

POST /api/users/buy
Description: Purchase products with deposited money.
Authentication: Requires buyer role.
Request Body:
JSON object with ProductId and Amount representing the purchased products.
Response:
Status Code: 200 OK
Body: Purchase details including total spent, products purchased, and change in coins.
Status Code: 400 Bad Request if the buyer or product is not found, or if there are insufficient funds.
