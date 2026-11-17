
Tokeny i limity modeli (Lekcja 3 / Zadanie 3.1)

Pomiary wykonane dnia 17 listopada 2026 r
Narzędzie / środowisko agenta: Antigravity (Model: Gemini 3.8 Flash).  

1. Liczba tokenów pierwszej wiadomości po polsku
Treść wiadomości:
  „Zbuduj minutnik w tym projekcie Avalonia. Ma odmierzać 25 minut pracy, potem 5 minut przerwy, i liczyć skończone cykle. Obie długości mają być do zmiany w oknie. Jeden przycisk start/pauza, drugi reset. Czas widoczny jako mm:ss, licznik cykli widoczny obok. Po skończonej pracy przerwa ma ruszyć sama. Interfejs po polsku, nazwy w kodzie po angielsku, bez dodatkowych bibliotek. Cała logika wprost w pliku MainWindow.axaml.cs, bez osobnych klas pośredniczących.”
Liczba znaków: 461 znaków, 68 słów
Wynik w tokenizatorze `cl100k_base`:163 tokeny
(Dla porównania w `o200k_base`: 146 tokenów)

2. Liczba tokenów pierwszej wiadomości po angielsku
Treść wiadomości:
  „Build a timer in this Avalonia project. It should measure 25 minutes of work, then 5 minutes of break, and count completed cycles. Both durations should be changeable in the window. One start/pause button, another reset. Time visible as mm:ss, cycle counter visible next to it. After finished work, the break should start automatically. Polish interface, English names in code, no additional libraries. All logic directly in the MainWindow.axaml.cs file, without separate intermediary classes.”
Liczba znaków: 493 znaki, 74 słowa
Wynik w tokenizatorze `cl100k_base`:100 tokenów (średnio ok. 4,93 znaku na token)
(Dla porównania w `o200k_base`: 101 tokenów)

3. Okno kontekstu modelu 
Wartość: 1 000 000 tokenów (1M tokenów)
Źródło (https://ai.google.dev/gemini-api/docs/gemini-3)

4. Maksymalna długość pojedynczej odpowiedzi
Wartość:65 536 tokenów (64k tokenów)
Źródło: (https://ai.google.dev/gemini-api/docs/gemini-3)

Wynik próby wpisania wartości dziesiętnych w pole długości (25,5 vs 25.5)

W aplikacji do wprowadzania długości trwania fazy zastosowano kontrolkę `NumericUpDown` ze standardowym formatem liczbowym. Test przeprowadzono w polskim środowisku regionalnym systemu Windows (`pl-PL`):

1.Wpisanie `25,5`
   Przy wciśnięciu Enter, ze względu na formatowanie liczb całkowitych (`FormatString="0"`), Avalonia zaokrągla wyświetlaną wartość do `26` (lub przy rzutowaniu na liczbę całkowitą pobiera `25`), a minutnik działa stabilnie bez wywołania błędu.

2.Wpisanie `25.5`
   Odrzuca ciąg znaków jako nieprawidłowy format liczbowy. Wartość w polu cofa się do ostatniej poprawnej liczby (np. `25`). Program nie ulega awarii, ale kropka nie pozwala na wprowadzenie ułamka.

