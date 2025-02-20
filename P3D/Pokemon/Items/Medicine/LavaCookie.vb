Namespace Items.Medicine

    <Item(7, "Lava Cookie")>
    Public Class LavaCookie

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "Lavaridge Town's local specialty. It can be used once to heal all the status conditions of a Pokemon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(192, 192, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pok�mon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.Status <> P3D.Pokemon.StatusProblems.None Or Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                    Pokemon.Status = P3D.Pokemon.StatusProblems.None

                    If Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        Pokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                    End If

                    Core.Player.Inventory.RemoveItem(Me.ID.ToString, 1)

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = Pokemon.GetDisplayName() & "~eats the cookie...*" & Pokemon.GetDisplayName() & "~gets healed up!"
                    t &= RemoveItem()
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    SoundManager.PlaySound("Use_Item", False)
                    Screen.TextBox.Show(t, {})

                    Return True
                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & "~doesn't want to eat~the cookie...", {})

                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
