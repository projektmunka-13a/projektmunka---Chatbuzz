using System;
using System.Collections.Generic;

namespace vizsgaremek_backend.Models;

public partial class Friendship
{
    public int Id { get; set; }

    public int RequesterId { get; set; }

    public int AddresseeId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User Addressee { get; set; } = null!;

    public virtual User Requester { get; set; } = null!;
}
