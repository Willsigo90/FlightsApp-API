using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaz
{
    public interface IServiceCurrency
    {
        public Task<CurrencyDTO> GetCurrency(string currency);
    }
}
