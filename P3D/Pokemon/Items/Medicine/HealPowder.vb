Namespace Items.Medicine

    <Item(123, "Heal Powder")>
    Public Class HealPowder

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "A very bitter medicine powder. When consumed, it heals all of a Pokémon's status conditions."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 450

        Public Sub New()
            _textureRectangle = New Rectangle(48, 120, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.Status <> P3D.Pokemon.StatusProblems.None Then
                    Pokemon.Status = P3D.Pokemon.StatusProblems.None
                    Pokemon.ChangeFriendShip(Pokemon.FriendShipCauses.HealPowder)

                    Core.Player.Inventory.RemoveItem(Me.ID.ToString, 1)

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = Pokemon.GetDisplayName() & "~gets healed up!"
                    t &= RemoveItem()

                    SoundManager.PlaySound("Use_Item", False)
                    Screen.TextBox.Show(t, {})
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    Return True
                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fully healed!", {})

                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
