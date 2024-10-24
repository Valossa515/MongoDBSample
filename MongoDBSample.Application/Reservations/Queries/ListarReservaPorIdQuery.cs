﻿using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Reservations.Data;

namespace MongoDBSample.Application.Reservations.Queries
{
    public class ListarReservaPorIdQuery
        : IQuery<IEnumerable<ListarReservaResponse>>
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
    }
}