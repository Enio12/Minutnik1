# Minutnik

Aplikacja minutnika (Pomodoro) napisana w C# i Avalonia UI, odmierzająca naprzemiennie czas pracy i przerwy oraz zliczająca ukończone cykle wraz z historią.

---

## Siedem zasad minutnika

1. **Czas pracy i przerwy**: Domyślnie 25 minut pracy i 5 minut przerwy. Obie wartości można swobodnie modyfikować w oknie za pomocą kontrolek liczbowych.
2. **Przyciski**: Jeden wspólny przycisk przełączający **Start / Pauza** oraz drugi przycisk **Reset** przywracający stan początkowy.
3. **Licznik cykli**: Wyraźnie widoczny licznik ukończonych pełnych cykli pracy.
4. **Automatyczna przerwa**: Po upływie czasu pracy odliczanie przerwy rusza automatycznie bez konieczności ponownego klikania Start. Po skończeniu przerwy kolejny cykl pracy również rusza samoczynnie.
5. **Format czasu**: Czas jest zawsze prezentowany w formacie `mm:ss` (np. `25:00`, `04:59`).
6. **Języki**: Interfejs użytkownika jest w całości po polsku, a identyfikatory w kodzie źródłowym po angielsku.
7. **Brak dodatkowych bibliotek**: Wykorzystano wyłącznie standardowe pakiety Avalonia UI oraz bibliotekę bazową .NET (m.in. `DispatcherTimer`), bez zewnętrznych zależności.
8. **Struktura**: Cała logika aplikacji znajduje się bezpośrednio w pliku `MainWindow.axaml.cs`, bez tworzenia osobnych klas pośredniczących.

---

## Tokeny i limity modeli (Lekcja 3 / Zadanie 3.1)

Pomiary wykonane dnia **17 listopada 2026 r.**  
Narzędzie / środowisko agenta: **Antigravity** (Model: **Gemini 3.8 Flash**).  
Do weryfikacji tokenów użyto standardowych tokenizatorów BPE (m.in. tokenizer `cl100k_base` będący domyślnym standardem w [Xenova The Tokenizer Playground](https://huggingface.co/spaces/Xenova/the-tokenizer-playground) oraz `o200k_base`):

### 1. Liczba tokenów pierwszej wiadomości po polsku
* **Treść wiadomości**:
  > *„Zbuduj minutnik w tym projekcie Avalonia. Ma odmierzać 25 minut pracy, potem 5 minut przerwy, i liczyć skończone cykle. Obie długości mają być do zmiany w oknie. Jeden przycisk start/pauza, drugi reset. Czas widoczny jako mm:ss, licznik cykli widoczny obok. Po skończonej pracy przerwa ma ruszyć sama. Interfejs po polsku, nazwy w kodzie po angielsku, bez dodatkowych bibliotek. Cała logika wprost w pliku MainWindow.axaml.cs, bez osobnych klas pośredniczących.”*
* **Liczba znaków**: 461 znaków, 68 słów
* **Wynik w tokenizatorze `cl100k_base` (GPT-4 / Xenova Playground)**: **163 tokeny** (średnio ok. 2,83 znaku na token)
* *(Dla porównania w `o200k_base`: 146 tokenów)*

### 2. Liczba tokenów pierwszej wiadomości po angielsku
* **Treść wiadomości**:
  > *„Build a timer in this Avalonia project. It should measure 25 minutes of work, then 5 minutes of break, and count completed cycles. Both durations should be changeable in the window. One start/pause button, another reset. Time visible as mm:ss, cycle counter visible next to it. After finished work, the break should start automatically. Polish interface, English names in code, no additional libraries. All logic directly in the MainWindow.axaml.cs file, without separate intermediary classes.”*
* **Liczba znaków**: 493 znaki, 74 słowa
* **Wynik w tokenizatorze `cl100k_base` (GPT-4 / Xenova Playground)**: **100 tokenów** (średnio ok. 4,93 znaku na token)
* *(Dla porównania w `o200k_base`: 101 tokenów)*

> **Wniosek**: Mimo że polski tekst ma mniej znaków (461 vs 493) i mniej słów (68 vs 74), rozbija się na aż 163 tokeny w porównaniu do 100 tokenów tekstu angielskiego (o ponad 60% więcej!). Wynika to ze specyfiki języka polskiego (znaki diakrytyczne, bogata fleksja), który w słownikach tokenizatorów jest rzadziej reprezentowany jako całe słowa.

### 3. Okno kontekstu modelu (Context Window)
* **Wartość**: **1 000 000 tokenów** (1M tokenów)
* **Źródło / Link**: [Dokumentacja modeli Gemini - Google AI](https://ai.google.dev/gemini-api/docs/gemini-3) *(por. stan dla Claude: [Dokumentacja Claude](https://platform.claude.com/docs/en/about-claude/models/overview))*

### 4. Maksymalna długość pojedynczej odpowiedzi (Max Output Tokens)
* **Wartość**: **65 536 tokenów** (64k tokenów)
* **Źródło / Link**: [Dokumentacja Gemini API - Limity wyjścia](https://ai.google.dev/gemini-api/docs/gemini-3) *(por. stan dla Claude: 128 000 / 64 000 tokenów na [Dokumentacja Claude Models](https://platform.claude.com/docs/en/about-claude/models/overview))*

---

## Wynik próby wpisania wartości dziesiętnych w pole długości (25,5 vs 25.5)

W aplikacji do wprowadzania długości trwania fazy zastosowano kontrolkę `NumericUpDown` ze standardowym formatem liczbowym. Test przeprowadzono w polskim środowisku regionalnym systemu Windows (`pl-PL`):

1. **Wpisanie `25,5` (z przecinkiem)**:
   * **Zachowanie**: W kulturze `pl-PL` przecinek jest prawidłowym separatorem dziesiętnym. Kontrolka poprawnie rozpoznaje wpisaną wartość jako liczbę dziesiętną `25.5`.
   * **Efekt końcowy**: Przy utracie fokusu (lub wciśnięciu Enter), ze względu na formatowanie liczb całkowitych (`FormatString="0"`), Avalonia zaokrągla wyświetlaną wartość do `26` (lub przy rzutowaniu na liczbę całkowitą pobiera `25`), a minutnik działa stabilnie bez wywołania błędu.

2. **Wpisanie `25.5` (z kropką)**:
   * **Zachowanie**: W kulturze `pl-PL` kropka nie jest separatorem dziesiętnym (w polskiej notacji kropka bywa traktowana jako separator tysięcy lub znak niedozwolony w liczbie).
   * **Efekt końcowy**: Walidator kontrolki odrzuca ciąg znaków jako nieprawidłowy format liczbowy. Wartość w polu cofa się do ostatniej poprawnej liczby (np. `25`) lub pole nie zatwierdza nowej wartości. Program nie ulega awarii, ale kropka nie pozwala na wprowadzenie ułamka.
