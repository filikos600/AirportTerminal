//paths are always odd and non-path even and there's more than one difference beside each sector(with and without path)
public enum SectorType
{
    Terminal = 0,
    TerminalPath = 1, 
        
    Security = 4,
    SecurityPath = 5, 

    Entry = 8,
    EntryPath = 9,

    Gates = 12, 
    GatesPath = 13,

    Service = 16,
    ServicePath = 17,

    Arrivals = 20,
    ArrivalsPath = 21,

    Schengen = 24,
    SchengenPath = 25,

    NonSchengen = 28,
    NonSchengenPath = 29,

    Bags = 32,
    BagsPath = 33,

    Empty = 36,
    
    BagClaim = 40,

    CheckIn = 44,
    CheckInPath = 45,

    Bench = 48,

    Outside = -1
}
