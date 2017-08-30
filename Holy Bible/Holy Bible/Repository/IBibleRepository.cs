using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holy_Bible.Domain;

namespace Holy_Bible.Repository
{
    interface IBibleRepository
    {
        IList<Book> GetAllBooks();

        Book GetRandomBook();

        IList<Verse> findVersesByText(string text);
    }
}
