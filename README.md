# MaxCars
Max cars provides maximum travel satisfaction!

## Timer (max. 3 hours)
Start Time: 20:23

End Time: 23:46 (Including database setup)

## Docker Support
To run this project in Docker simply call `docker-compose -p max-cars up` in the main directory.

* Booking service: https://localhost:8001/swagger/index.html
* Pricing service: https://localhost:9001/swagger/index.html

## Execution
You can use the `Try it out` button in the swagger view to test the functionality of the booking
and pricing service.

The following cars are seeded into the DB:

`
(1, 'Mercedes', 50),
(2, 'BMW',  60),
(3, 'Nissan', 20);
`

Good luck!