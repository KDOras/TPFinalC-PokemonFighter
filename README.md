# PokemonFighter


Fonctionalité :

- CreateUser
- Login
- Pokedex de Choix ( les pokemons on directement les attaques en dessous du coup pas de page pour les attaques car il y'a marquer les degat et les nom)
- Combat :
  - Barre de vie
  - Compteur de Victoire
  - 4 Attaque Max (2 sur tout les pokemons pour plus d'amusement et moins de situation impossible à gagner ou trop simple)
  - Boost sur votre pokemon à chaque victoire de 0.05 soit sur les PV soit sur l'attaque ( aléatoire entre les 2 )
  - Boost sur l'adversaire à chaque victoire de 0.15 soit sur les PV soit sur l'attaque ( aléatoire entre les 2 )


De Plus si vous voulez vous amusez sur les détails :

- 3 img de depart ( aléatoire )
- 3 img de combat ( aléatoire )

- Systeme d'évolution à moitié mis en gros si vous arrivé à 10 victoire ou + vous avez une possibilité que votre pokemon deviennent sont evolution
(Je ne sais pas vraiment si ça marche car ça a marché une fois sur 2)

Pour le bien du projet penser à aller dans :

ExerciceMonsterContext.cs et modifier le text entre "Localhost\\ etc" car je n'ai pas eu le temps de reussir cela donc il est fonctionelle de mon cté mais j'ai pas réussi à implementer de changer cette partie ça endommager tout le reste de mon code à chaque fois
ET
GlobalVar.cs sur une autre ligne dans CleanData qui utilise la même ligne
