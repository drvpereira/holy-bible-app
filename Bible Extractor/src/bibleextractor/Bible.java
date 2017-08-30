package bibleextractor;

import java.util.ArrayList;
import java.util.List;

public class Bible {

	private String translation;
	
	private String language;
	
	private List<Book> books = new ArrayList<Book>();

	public String getTranslation() {
		return translation;
	}

	public void setTranslation(String translation) {
		this.translation = translation;
	}

	public List<Book> getBooks() {
		return books;
	}

	public void setBooks(List<Book> books) {
		this.books = books;
	}

	public String getLanguage() {
		return language;
	}

	public void setLanguage(String language) {
		this.language = language;
	}

}
