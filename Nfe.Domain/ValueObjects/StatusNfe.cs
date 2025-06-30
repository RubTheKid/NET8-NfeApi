using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nfe.Domain.ValueObjects;

public enum StatusNFe
{
    EmProcessamento = 1,
    Autorizada = 2,
    Rejeitada = 3,
    Cancelada = 4
}
