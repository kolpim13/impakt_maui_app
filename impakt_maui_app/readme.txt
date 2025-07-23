1. Remake Popup ¨NewMember¨ as Page.
2. Profile Page | ? [Edit profile; Add member; Add stuff member]
3. Clean Up old but unused code anymore [Checkin History]
4. Add statistics windows [Self statistic during the day | ]
5. Be aligned with names with backend
6. Add Scanner types [Member`s info, Update Pass].

7. Make code of the ExternalProvider Page be aligned with PassTypes Page!!!
8. Remake new Member Page (?): Check what is absent and add it.
9. Add MemberPass scan
10. CheckIn
11. Clean "Main Page" completely
12. [+] Get rid of "John Doe" on profile`s Page




How to invoke event form VM in CodeBehind
[RelayCommand]
public void AddProvider()
{
    Model_ExternalProvider provider = new Model_ExternalProvider();
    OnAddNewProviderRequested?.Invoke(this, provider);
}
public event EventHandler<Model_ExternalProvider>? OnAddNewProviderRequested;

public void ShowContextMenu(Model_ExternalProvider provider)
{
    if (provider is null) return;
    OnShowContextMenuRequested?.Invoke(this, provider);
}
public event EventHandler<Model_ExternalProvider>? OnShowContextMenuRequested;

How to add long press to the CollectionView
<Border.Behaviors>
    <toolkit:TouchBehavior LongPressCommand="{Binding BindingContext.ShowContextMenuCommand, Source={x:Reference Page_ExternalProvider_Inst}}"
                           LongPressCommandParameter="{Binding BindingContext.SelectedProvider, Source={x:Reference Page_ExternalProvider_Inst}}" />
</Border.Behaviors>



<ScrollView>
        <VerticalStackLayout Padding="12" Spacing="8">
        <!--Buttons-->
        <Button Text="Add External Provider" Command="{Binding AddProviderCommand}" />
        
        <HorizontalStackLayout>
            <Button Text="Edit" Command="{Binding EditProviderCommand}" />
            <Button Text="Delete" Command="{Binding DeleteProviderCommand}" />
        </HorizontalStackLayout>

        <!--Scrollable External Providers List-->
            <CollectionView ItemsSource="{Binding Providers}" 
                            SelectionMode="Single"
                            SelectedItem="{Binding SelectedProvider}">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <Border Stroke="Gray" StrokeThickness="1" Background="#f7f7f7" Padding="12" Margin="6" StrokeShape="RoundRectangle 12">
                            <VerticalStackLayout>
                                <Label Text="{Binding name}" FontAttributes="Bold" FontSize="18"/>
                                <Label Text="{Binding description, Converter={StaticResource Converter_Description}}" 
                                       FontAttributes="Italic" FontSize="14"/>
                                <Label Text="{Binding active, StringFormat='Active: {0}'}" FontSize="13"/>
                                <Label Text="{Binding partial_payment, Converter={StaticResource Converter_PartialPayment}, StringFormat='Partial payment: {0}'}" 
                                       FontSize="13"/>
                            </VerticalStackLayout>
                        </Border>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
        </VerticalStackLayout>
    </ScrollView>