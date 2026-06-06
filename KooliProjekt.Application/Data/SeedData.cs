using System;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// Testandmete generaator
    /// Genereerib andmed ainult siis kui vajalik tabel on tühi.
    /// </summary>
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Genereerib andmed
        /// </summary>
        public void Generate()
        {
            // Kui juba on kliente, eeldame, et andmed on olemas -> ära dubleerime
            if (_dbContext.Kliendid.Any())
                return;

            // 1) Kliendid (10)
            for (var i = 1; i <= 10; i++)
            {
                var k = new Klient
                {
                    FirstName = "KliendiEesnimi" + i,
                    LastName = "Perekonnanimi" + i,
                    Email = $"client{i}@test.ee",
                    Address = $"Tänav {i}",
                    Phone = $"5000{i:000}",
                    Discount = i % 2 == 0 ? 5m : 0m
                };
                _dbContext.Kliendid.Add(k);
            }
            _dbContext.SaveChanges();

            // 2) Tooted (10)
            for (var i = 1; i <= 10; i++)
            {
                var t = new Toode
                {
                    Name = $"Toode {i}",
                    Description = $"Kirjeldus tootele {i}",
                    FotoURL = $"https://example.com/product{i}.jpg",
                    Price = 10m * i,
                    StockQuantity = 50m + i
                };
                _dbContext.Tooted.Add(t);
            }
            _dbContext.SaveChanges();

            // 3) Tellimused (10) - loome iga kliendi + tellimuse
            var klientIds = _dbContext.Kliendid.Select(x => x.Id).ToList();
            var toodeIds = _dbContext.Tooted.Select(x => x.Id).ToList();

            for (var i = 1; i <= 10; i++)
            {
                var tellimus = new Tellimus
                {
                    KlientId = klientIds[(i - 1) % klientIds.Count],
                    OrderDate = DateTime.Now.AddDays(-i),
                    Status = i % 2 == 0 ? "Completed" : "Pending"
                };
                _dbContext.Tellimused.Add(tellimus);
            }
            _dbContext.SaveChanges();

            // 4) TellimuseRead (iga tellimuse kohta vähemalt 1 rida; kokku 10+)
            var tellimusIds = _dbContext.Tellimused.Select(t => t.Id).ToList();

            var rnd = new Random();
            foreach (var tid in tellimusIds)
            {
                // lisa 1-3 rida iga tellimuse juurde
                var rowsCount = 1 + rnd.Next(0, 3);
                for (var r = 0; r < rowsCount; r++)
                {
                    var prodId = toodeIds[rnd.Next(0, toodeIds.Count)];
                    var qty = 1 + rnd.Next(0, 5);
                    var unitPrice = _dbContext.Tooted.Find(prodId).Price;
                    var lineTotal = unitPrice * qty;
                    var vatRate = 0.20m;
                    var vatAmount = lineTotal * vatRate;

                    var rida = new TellimuseRida
                    {
                        TellimusId = tid,
                        ToodeId = prodId,
                        Quantity = qty,
                        UnitPrice = unitPrice,
                        LineTotal = lineTotal,
                        VatRate = vatRate,
                        VatAmount = vatAmount
                    };
                    _dbContext.TellimusedRida.Add(rida);
                }
            }
            _dbContext.SaveChanges();

            // 5) Arved (loo arv iga tellimuse jaoks; 10 arvet)
            foreach (var tid in tellimusIds)
            {
                // leia seotud read ja arvuta subtotal + käibemaks
                var read = _dbContext.TellimusedRida.Where(r => r.TellimusId == tid).ToList();
                var subTotal = read.Sum(r => r.LineTotal);
                var vatSum = read.Sum(r => r.VatAmount);

                var arve = new Arve
                {
                    KlientId = _dbContext.Tellimused.Find(tid).KlientId,
                    TellimusId = tid,
                    InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{tid}",
                    InvoiceDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    Status = "Pending",
                    SubTotal = subTotal,
                    ShippingTotal = 0m,
                    Discount = 0m,
                    GrandTotal = subTotal + vatSum
                };
                _dbContext.Arved.Add(arve);
            }
            _dbContext.SaveChanges();
        }
    }
}