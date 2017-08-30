package bibleextractor;

import java.util.List;

public class Book {

	private String name;
	
	private String acr;
	
	private String fn;
	
	private int nc;
	
	private List<Chapter> chapters;

	public Book() {
		
	}
	
	public Book(String name, String acr) {
		this.name = name;
		this.acr = acr;
	}
	
	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getAcr() {
		return acr;
	}

	public void setAcr(String acr) {
		this.acr = acr;
	}

	public String getFn() {
		return fn;
	}

	public void setFn(String fn) {
		this.fn = fn;
	}

	public List<Chapter> getChapters() {
		return chapters;
	}

	public void setChapters(List<Chapter> chapters) {
		this.chapters = chapters;
	}

	public int getNc() {
		return nc;
	}

	public void setNc(int nc) {
		this.nc = nc;
	}
	
}
