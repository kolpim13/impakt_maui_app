ToDo:
1. BugFixes:
    * Page_Statistics_Admin_InstructorsStatistics:
        * Not allow to get data of DateFrom less than DateTo
        * Validate on empty data if returned so.
2. New Functionalities:
    * Add mail verification.
    * Add personal tokens to send before every request.
    * Add privilage / access / whatever system. So every user will have its personal allowance written in DB.
    * New "Statistics" Views:
        * For the instructor, so he can validate himself.
    * Give possibility to generate QR code straight away in application.
    * Generation of the QR Code [self, for the user].
3. Updates:
    * Update Profile Page - Add possibility to update personal data || change password || change mail.
    * Statistics: Page with CheckIn Details should be able to:
        * [+] Regroup data [hour / day / week / month]                  
        * Color data - for example, red row for rejected entries.
        * [+] Show short summary in footer - total, accepted, rejected.
        * When view is empty [No data was taken from DB], shows some text or progress bar.
    * QR Codes:
        * Different colors for different roles (Members - Black, Instructors - Green, etc.).
4. Refacrtoring
    *
5. Ideas
    * Think about what data can be moved under "toolbar".



How to invoke event form VM in CodeBehind
[RelayCommand]
public void AddProvider()
{
    ExternalProvider provider = new ExternalProvider();
    OnAddNewProviderRequested?.Invoke(this, provider);
}
public event EventHandler<ExternalProvider>? OnAddNewProviderRequested;

public void ShowContextMenu(ExternalProvider provider)
{
    if (provider is null) return;
    OnShowContextMenuRequested?.Invoke(this, provider);
}
public event EventHandler<ExternalProvider>? OnShowContextMenuRequested;

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