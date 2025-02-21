using System;
using System.Collections.Generic;

namespace NotesApi.Models;

public partial class Note
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
