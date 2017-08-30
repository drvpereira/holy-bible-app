package bibleextractor;

import java.util.List;

public class Chapter {

	private int num;
	
	private List<Verse> verses;

	public Chapter() {
		
	}
	
	public Chapter(int num) {
		this.num = num;
	}
	
	public int getNum() {
		return num;
	}

	public void setNum(int num) {
		this.num = num;
	}

	public List<Verse> getVerses() {
		return verses;
	}

	public void setVerses(List<Verse> verses) {
		this.verses = verses;
	}
	
}
