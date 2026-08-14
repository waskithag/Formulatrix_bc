```mermaid
classDiagram
    class Card {
        +Color color : readonly
        +CardValue value : readonly
    }

    class Player{
        +string name : readonly 
    }

    class Deck{
        +Stack~Card~ deckPiles  
        +Deck()
    }

    class Discarded{
        +Stack~Card~ discardPiles
    }

    class GameController{
        -List~Player~  _players
        -Dictionary~Player, List~Card~~ _cardInHand  
        -Deck _deck 
        -Dicarded _discardPile
        -GameDirection _gameDirection 
        -int _currentPlayerIndex 
        -bool _turnSkipped = false 

        +Play() : void
        +AddPlayer() : void
        +DistributeCard() : void
        +PlayerTurn(Player player) : void

        +CheckPlayableCard(Player player) : List~int~
        +CheckCardPlaibility(Card card) : bool
        +PlayCard(Card card) : void
        +CheckIfWinner(Player player) : bool
        +CheckPlayedCard(Card card) : void
        +SpecialCardPlayed() : void

        +NextTurn(bool skipped) : void

        +DrawCard(Player) : void
        +DiscardCard(Card card) : void
        +Shuffle() : void
        +RenewDeck() : void

        +isUno() : bool
        +CallUno() : void

        +GetCurrentPlayerIndex() : int
        +GetCurrentTopPile() : card
        +GetPlayerCard(Player player) : List~Card~
     }

    class Color{
        <<enumeration>>
        red,
        green,
        blue,
        yellow,
        wild
    }
    
    class CardValue{
        <<enumeration>>
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        skip,
        reverse,
        plusTwo,
        plusFour,
        wild
    }

    class GameDirection{
        <<enumeration>>
        clockwise,
        counterClockwise
    }

    Deck *-- Card
    Discarded *-- Card 

    Card *-- Color
    Card *-- CardValue
    
    GameController <-- Player
    GameController <-- Deck
    GameController <-- Discarded
    GameController *-- GameDirection
```
