﻿#language: de-DE
Funktionalität: Hkpv - Validierung der gemeldeten Aktivitäten der Datenmeldung

Szenariogrundriss: Eine Aktivität 2 oder 3 aber keine Leistung 4-17 zu dieser Aktivität, ab dem Jahr 2019 ist es ein Fehler.
    Angenommen die Meldung enthält die Aktivitäten '<Art>'
	Und die Eigenschaft 'to' von 'HkpvReport' ist auf '2019-01-01' gesetzt
	Dann enthält das Validierungsergebnis die Warnung 'Kein Eintrag '4-17''
Beispiele:
    | Art      |	
    | 2        |
    | 3        |	

Szenariogrundriss: Eine Aktivität 2 oder 3 aber keine Leistung 4-17 zu dieser Aktivität, bis zum Jahr 2019 ist es korrekt.
    Angenommen die Meldung enthält die Aktivitäten '<Art>'
	Und die Eigenschaft 'to' von 'HkpvReport' ist auf '2018-12-31' gesetzt
    Dann enthält das Validierungsergebnis nicht den Fehler 'Kein Eintrag '4-17''
Beispiele:
    | Art      |	
    | 2        |
    | 3        |	

Szenariogrundriss: Eine Aktivität 4-14, 16-17 aber keine 1,2 oder 3.
    Angenommen die Meldung enthält die Aktivitäten '<Art>'
    Dann enthält das Validierungsergebnis den Fehler 'Kein Eintrag '1,2,3''
Beispiele:
    | Art      | 
    | 4        | 
    | 5        |
    | 6        |
    | 7        |
    | 8        |
    | 9        |
    | 10       |
    | 11       |
    | 12       |
    | 13       |
    | 14       |	
    | 16       |
    | 17       |

Szenariogrundriss: Eine Auszubildende hat medizinische Leistungen dokumentiert.
    Angenommen eine Auszubildende hat die Aktivitäten '<Art>' dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'darf als Auszubildende/r keine medizinischen Leistungen'
Beispiele:
    | Art |
    | 6   |
    | 7   |
    | 8   |
    | 9   |
    | 10  |	

Szenariogrundriss: Beispiele für gültige Aktivitäten
    Angenommen die Meldung enthält die Aktivitäten '<Art>'	
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen
Beispiele:
    | Art       |
    | 1         |
    | 2,6       |
    | 15        |  
    | 2,15      |	
    | 2,2,15    |  # zwei Hausbesuche an einem Tag

@diskussion
@ignore
Szenario: Es ist nur ein Hausbesuch pro Aktivitätsblock zulässig
    Angenommen die Meldung enthält die Aktivitäten '02,02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Es ist nur ein Hausbesuch pro Aktivitätsblock zulässig'	

Szenariogrundriss: Beispiele für möglicherweise ungültige Aktivitäten
    Angenommen die Meldung enthält die Aktivitäten '<Art>'	
	Und die Eigenschaft 'to' von 'HkpvReport' ist auf '<Datum>' gesetzt
    Dann enthält das Validierungsergebnis die Warnung '<Fehler>'
Beispiele:
    | Art | Datum		 | Fehler |
    | 2   |2019-01-20    |Kein Eintrag '4-17' |

Szenariogrundriss: Beispiele für ungültige Aktivitäten
    Angenommen die Meldung enthält die Aktivitäten '<Art>'	
    Dann enthält das Validierungsergebnis den Fehler '<Fehler>'	
Beispiele:
    | Art    | Fehler                         |
    | 0      | darf nicht 'UndefinedActivity' sein. |

@weich
Szenario: Eine Mitarbeiter hat eine Aktivität 1-17 öfter als 5 Mal an einem Tag bei einem Klienten.
    Angenommen die Meldung enthält die Aktivitäten '2,15,15,15,15,15,15'		
    Dann enthält das Validierungsergebnis die Warnung 'Es wurden mehr als 5 gemeldet.'

Szenario: Eine Mitarbeiter hat eine Aktivität 31 oder 33 öfter als 5 Mal an einem Tag.
    Angenommen die Meldung enthält bei der Person '' die Aktivitäten '31,31,31,31,31,31,31'
        Und die Meldung enthält die Aktivitäten '2,15'	
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen

@weich
Szenario: Ein Klienten hat jeden Tag Leistungen dokumentiert, in Summe mehr als 350 LP.
    Angenommen die Meldung enthält jeden Tag die Aktivitäten '2,04,04,04,15'
    Dann enthält das Validierungsergebnis die Warnung 'wurden mehr als 350 LP in einem Monat erfasst.'

Szenario: Ein Klienten hat innerhalb kurzer Zeit mehr als 350 LP gemeldet.
    Angenommen die Meldung enthält am '2008-04-30' die Aktivitäten '02,04,04,04,04,05,05,05,05,06,06,06,07,07,07,08,08,08,09,09,09,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,17,17,17'	
    Angenommen die Meldung enthält am '2008-04-29' die Aktivitäten '02,04,04,04,04,05,05,05,05,06,06,06,07,07,07,08,08,08,09,09,09,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,17,17,17'	
    Angenommen die Meldung enthält am '2008-04-28' die Aktivitäten '02,04,04,04,04,05,05,05,05,06,06,06,07,07,07,08,08,08,09,09,09,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,17,17,17'	
    Angenommen die Meldung enthält am '2008-04-27' die Aktivitäten '02,04,04,04,04,05,05,05,05,06,06,06,07,07,07,08,08,08,09,09,09,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,17,17,17'	
    Dann enthält das Validierungsergebnis die Warnung 'wurden mehr als 350 LP in einem Monat erfasst.'

Szenario: Eine Aktivität ist nach dem Meldungszeitraum.
    Angenommen die Meldung enthält am '2058-04-30' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss kleiner oder gleich (.*) sein'

Szenario: Eine Aktivität ist vor dem Meldungszeitraum.
    Angenommen die Meldung enthält am '2008-04-30' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss grösser oder gleich (.*) sein.'

Szenario: Eine Aktivität ohne entsprechenden Eintrag in Persons.
    Angenommen die Meldung enthält bei der Person 'unbekannteId' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

Szenario: Eine Aktivität 1-17 ohne Klientenbezug.
    Angenommen die Meldung enthält bei der Person '' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler ''PersonId' darf nicht leer sein.'

Szenario: Eine Aktivität 31,33 mit Klientenbezug.
    Angenommen die Meldung enthält bei der Person '1' die Aktivitäten '31'
    Dann enthält das Validierungsergebnis den Fehler ''PersonId' sollte leer sein.'

Szenario: Eine Aktivität ohne entsprechenden Eintrag in Staffs.
    Angenommen die Meldung enthält von der Mitarbeiterin 'unbekannteId' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

Szenario: Eine Person ohne Aktivität.
    Angenommen zu einer Person sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenario: Eine Mitarbeiterin ohne Aktivität.
    Angenommen zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung |
    | date        | Datum       |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'Activity' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name | Bezeichnung |
    | date | Datum       |