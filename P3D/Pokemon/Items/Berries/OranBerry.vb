Namespace Items.Berries

    <Item(2006, "Oran")>
    Public Class OranBerry

        Inherits Berry

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True

        Public Sub New()
            MyBase.New(10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can restore its own HP by 10 points during battle.", "3.5cm", "Super Hard", 2, 3)

            Me.Spicy = 10
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Poison
            Me.Power = 80
            Me.JuiceColor = "blue"
            Me.JuiceGroup = 1
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItems", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealPokemon(PokeIndex, 10)
        End Function

    End Class

End Namespace
