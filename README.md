# Alten Hotel Test API
## Coded by Ricardo Leyva

Written in .NET 6, EntityFrameworkCore, RestFul Architecture

## Features

- Clients insertion.
- Reservation insertion.
- Dates and room availability validations based on what was required.
- InMemory Database for testing purposes, not SQLServer required.
 
## Improvements that could be done, given enough time
- Proper Json Formatting on responses.
- Custom models (POST and PUT) for request model binding.
- Data validation on model binding.
- Model lazy loading population given URI's like /client/{id}/reservation

# API Structure
## Schemas

### Client
```sh
{
    id: int32,
    name: string,
    identification: string
}
```
### Reservation
```sh
{
    id: int32,
    clientId: int32(nullable),
    startReservation: Datetime(yyyy-MM-dd),
    endReservation: Datetime(yyyy-MM-dd),
    reservationDate: Datetime(yyyy-MM-dd),
    code: string(nullable),
    state:	ReservationStateinteger($int32),
    paymentMethod:	PaymentMethodinteger($int32),
}
```
### ReservationState
```sh
{
    0 = "Active",
    1 = "Inactive",
    2 = "Canceled"
}
```

### PaymentMethod
```sh
{
    0 = "Cash",
    1 = "Transfer",
    2 = "CreditCard"
}
```

## EndPoints
### Clients
#### GET
> /api/Client
~~~~
No Parameters
~~~~
> /api/Client/{id}
~~~~
No Parameters
~~~~
#### POST
> /api/Client
~~~~
{
    "name": "string",
    "identification": "string"
}
~~~~
#### PUT
>/api/Client/{id}
~~~~
{
    "name": "string",
    "identification": "string"
}
~~~~
#### DELETE
> /api/Client/{id}
~~~~
No Parameters
~~~~

### Reservation
#### GET
> /api/Reservation?code={code}
~~~~
{
    "code": "string?"
}
~~~~
> /api/Reservation/{id}
~~~~
No Parameters
~~~~
#### POST
> /api/Reservation
~~~~
{
    "clientId": 0?,
    "startReservation": "2022-08-24",
    "endReservation": "2022-08-24",
    "state": 0?,
    "paymentMethod": 0?,
}
~~~~
#### PUT
>/api/Reservation/{id}
~~~~
{
    "clientId": 0?,
    "startReservation": "2022-08-24",
    "endReservation": "2022-08-24",
    "state": 0?,
    "paymentMethod": 0?,
}
~~~~
#### DELETE
> /api/Reservation/{id}
~~~~
No Parameters
~~~~


