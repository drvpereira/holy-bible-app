package bibleextractor;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.net.URL;
import java.net.URLConnection;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.lang3.StringEscapeUtils;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.core.util.Base64Encoder;
import com.thoughtworks.xstream.io.xml.DomDriver;

public class Extractor {

	private static final String URL_TEMPLATE = "http://www.bibliaonline.com.br/%s/%s/%d"; 
	
	private String translation;
	
	private String[] bookNames = { "Genesis", "Exodus", "Leviticus", "Numbers", "Deuteronomy", "Joshua", "Judges", "Ruth", "1 Samuel", "2 Samuel", "1 Kings", "2 Kings",
			"1 Chronicles", "2 Chronicles", "Ezra", "Nehemiah", "Esther", "Job", "Psalms", "Proverbs", "Ecclesiastes", "Song of Songs", "Isaiah", "Jeremiah", "Lamentations",
			"Ezekiel", "Daniel", "Hosea", "Joel", "Amos", "Obadiah", "Jonah", "Micah", "Nahum", "Habakkuk", "Zephaniah", "Haggai", "Zechariah", "Malachi", "Matthew", "Mark",
			"Luke", "John", "Acts", "Romans", "1 Corinthians", "2 Corinthians", "Galatians", "Ephesians", "Philippians", "Colossians", "1 Thessalonians", "2 Thessalonians",
			"1 Timothy", "2 Thimoty", "Titus", "Philemon", "Hebrews", "James", "1 Peter", "2 Peter", "1 John", "2 John", "3 John", "Jude", "Revelations" };
	
	private String[] bookAcrs = { "gn", "ex", "lv", "nm", "dt", "js", "jz", "rt", "1sm", "2sm", "1rs", "2rs", "1cr", "2cr", "ed", "ne", "et", "jó", "sl", "pv", "ec",
			"ct", "is", "jr", "lm", "ez", "dn", "os", "jl", "am", "ob", "jn", "mq", "na", "hc", "sf", "ag", "zc", "ml", "mt", "mc", "lc", "jo", "atos", "rm",
			"1co", "2co", "gl", "ef", "fp", "cl", "1ts", "2ts", "1tm", "2tm", "tt", "fm", "hb", "tg", "1pe", "2pe", "1jo", "2jo", "3jo", "jd", "ap"};
	
	public Bible extractBible(String transAcr, String transName, String lang) throws Exception {
		this.translation = transAcr;
		
		Bible bible = new Bible();
		bible.setTranslation(transName);
		
		List<Book> books = new ArrayList<Book>();
		int i = 0;
		for (String bookAcr : bookAcrs) {
			System.out.println("Pegando " + bookAcr);
			long time = System.currentTimeMillis();
			Book book = getBook(bookAcr, bookNames[i++]);
			books.add(book);
			
			bookToXML(book, transAcr);
			
			System.out.println((System.currentTimeMillis() - time) + "ms");
		}
		
		bible.setLanguage(lang);
		bible.setBooks(books);
		return bible;
	}
	
	private Book getBook(String acr, String name) throws Exception {
		int chapterCount = getChapterCount(acr);
		
		Book b = new Book();
		b.setAcr(acr);
		b.setFn(acr);
		b.setName(name);
		b.setChapters(getChapters(acr, chapterCount));
		return b;
	}
	
	private List<Chapter> getChapters(String book, int chapterCount) throws Exception {
		List<Chapter> chapters = new ArrayList<Chapter>();
		for (int i = 1; i <= chapterCount; i++) {
			System.out.println("- Cap. " + i);
			Chapter chapter = new Chapter(i);
			List<Verse> verses = getVerses(book, i);
			chapter.setVerses(verses);
			chapters.add(chapter);
		}
		
		return chapters;
	}
	
	private List<Verse> getVerses(String book, int chapter) throws Exception {
		List<Verse> verses = new ArrayList<Verse>();
		URL url = new URL(String.format(URL_TEMPLATE, translation, book, chapter));
		String text = getText(url);

		Pattern pattern = Pattern.compile("<p([^>]+)>((.|\n)+?)</p>", Pattern.CASE_INSENSITIVE);
		Matcher matcher = pattern.matcher(text);
		int i = 0;
		while(matcher.find()) {
			String line = text.substring(matcher.start(), matcher.end());
			
			int textStart = line.indexOf('>');
			int textEnd = line.lastIndexOf('<');
			
			String verseLine = StringEscapeUtils.unescapeHtml4(line.substring(textStart + 1, textEnd).replaceAll("\n", ""));
			Verse verse = new Verse();
			verse.setNum(++i);
			verse.setTxt(verseLine);
			
			verses.add(verse);
		}
		
		return verses;
	}
	
	private String getText(URL url) throws Exception {
		
		System.setProperty("http.proxyHost", "inet-rj.petrobras.com.br");  
        System.setProperty("http.proxyPort", "8080");  
        String userpass = "u403:dmspTIP3";  
        String encodedLogin = new Base64Encoder().encode(userpass.getBytes());  

        URLConnection con = url.openConnection();  
        con.setRequestProperty("Proxy-Authorization", "Basic " + encodedLogin);
		con.setRequestProperty("User-Agent", "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.4; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2");
		
		StringBuilder sb = new StringBuilder();
		Scanner sc = new Scanner(con.getInputStream());

		while(sc.hasNextLine()) {
			sb.append(sc.nextLine() + "\n");
		}
		
		return sb.toString();
	}
	
	private int getChapterCount(String book) throws Exception {
		String starterDiv = "<div class='chapter_index'>";
		URL url = new URL(String.format(URL_TEMPLATE, translation, book, 1));

		String text = getText(url);		
		
		int startIndex = text.indexOf(starterDiv) + starterDiv.length();
		String chapterIndex = text.substring(startIndex);
		int endIndex = chapterIndex.indexOf("</div>");
		if (endIndex == -1) {
			System.out.println(chapterIndex);
		}
		chapterIndex = chapterIndex.substring(0, endIndex).trim();
		
		String[] split = chapterIndex.split("\n");
		return split.length;
	}
	
	public static void main(String[] args) throws Exception {
		String transAcr = "aa";
		String transName = "Almeida Revisada Imprensa Bíblica";
		
		Bible bible = new Extractor().extractBible(transAcr, transName, "Português do Brasil");
		
		XStream xs = new XStream(new DomDriver());
		xs.alias("bible", Bible.class);
		xs.alias("book", Book.class);
		xs.addImplicitCollection(Bible.class, "books");
		xs.useAttributeFor(Bible.class, "language");
		xs.useAttributeFor(Bible.class, "translation");
		xs.useAttributeFor(Book.class, "acr");
		xs.useAttributeFor(Book.class, "nc");
		xs.useAttributeFor(Book.class, "fn");
		xs.useAttributeFor(Book.class, "name");
		xs.omitField(Book.class, "chapters");
		
		xs.toXML(bible, new FileOutputStream(new File("./data/" + transAcr + "/bible.xml")));
	}
	
	private static void bookToXML(Book book, String trans) throws FileNotFoundException {
		book.setNc(book.getChapters().size());
		
		XStream xs = new XStream(new DomDriver());
		xs.alias("b", Book.class);
		xs.alias("c", Chapter.class);
		xs.alias("v", Verse.class);
		
		xs.addImplicitCollection(Book.class, "chapters");
		xs.addImplicitCollection(Chapter.class, "verses");

		xs.omitField(Book.class, "fn");
		
		xs.useAttributeFor(Book.class, "acr");
		xs.useAttributeFor(Book.class, "nc");
		xs.useAttributeFor(Book.class, "name");
		xs.useAttributeFor(Chapter.class, "num");
		xs.useAttributeFor(Verse.class, "num");
		xs.useAttributeFor(Verse.class, "txt");
		
		if (!new File("./data").exists()) {
			new File("./data").mkdir();
		}
		
		if (!new File("./data/" + trans).exists()) {
			new File("./data/" + trans).mkdir();
		}
		
		xs.toXML(book, new FileOutputStream(new File("./data/" + trans + "/" + book.getAcr() + ".xml")));
	}
	
}
