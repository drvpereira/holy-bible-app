package bibleverifier;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;

import bibleextractor.Bible;
import bibleextractor.Book;
import bibleextractor.Chapter;
import bibleextractor.Verse;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;

public class Verifier {

	public static void main(String[] args) throws FileNotFoundException {
		
		XStream xs = new XStream(new DomDriver());
		xs.alias("bible", Bible.class);
		xs.alias("book", Book.class);
		xs.alias("chapter", Chapter.class);
		xs.alias("verse", Verse.class);
		
		xs.addImplicitCollection(Bible.class, "books");
		xs.addImplicitCollection(Book.class, "chapters");
		xs.addImplicitCollection(Chapter.class, "verses");

		xs.useAttributeFor(Bible.class, "translation");
		xs.useAttributeFor(Book.class, "acr");
		xs.useAttributeFor(Book.class, "name");
		xs.useAttributeFor(Chapter.class, "num");
		xs.useAttributeFor(Verse.class, "num");
		xs.useAttributeFor(Verse.class, "txt");
		
		Bible bible = (Bible) xs.fromXML(new FileInputStream(new File("/home/david/bible_cuv.xml")));
		int erros = 0;
		
		if (bible.getBooks() == null || bible.getBooks().isEmpty()) {
			System.out.println("ERRO: lista de livros vazia!");
			erros++;
		} else {
			for (Book book : bible.getBooks()) {
				System.out.println(book.getAcr() + " - " + book.getName());
				if (book.getChapters() == null || book.getChapters().isEmpty()) {
					 System.out.println("ERRO: lista de capítulos vazia para o livro " + book.getName() + "(" + book.getAcr() + ")");
					 erros++;
				} else {
					for (Chapter chapter : book.getChapters()) {
						
						if (chapter.getVerses() == null || chapter.getVerses().isEmpty()) {
							System.out.println("ERRO: lista de versículos vazia para o livro " + book.getName() + "(" + book.getAcr() + ") capítulo " + chapter.getNum());
							erros++;
						} else {
							for (Verse verse : chapter.getVerses()) {
								if (verse.getTxt() == null || verse.getTxt().trim().isEmpty()) {
									System.out.println("ERRO: versículo " + verse.getNum() + " do capítulo " + chapter.getNum() + " do livro " + book.getName() + "(" + book.getAcr() + ") está vazio!");
									erros++;
								}
							}
						}
						
					}
				}
				
			}
		}
		
		System.out.println("Total de erros: " + erros);
	}
	
}
