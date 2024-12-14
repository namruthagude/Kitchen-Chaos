using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField]
    private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField]
    private BurningRecipeSO[] burningRecipeSOArray;


    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private float fryTimer;
    private FryingRecipeSO fryingRecipeSO;
    private State state;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;


    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // If object frying
                        fryTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        Debug.Log("fried");
                    }
                    break;
                case State.Fried:
                     burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)burningTimer / burningRecipeSO.burnTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burnTimerMax)
                    {
                        // If object frying
                        burningTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }
                    break;
                case State.Burned:
                    break;
            }           
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object on top
            if (player.HasKitchenObject())
            {
                //Checking if we have input recipe 
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    //If player carrying kitchen object thT can cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                   fryingRecipeSO = GetFryingRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    fryTimer = 0f;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryTimer / fryingRecipeSO.fryingTimerMax
                    });
                }      
            }
        }
        else
        {
            //There is Kitchen Object on top
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player holding a plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private KitchenObjectsSO OutputForInputFryingRecipe(KitchenObjectsSO kitchenObjectsSO)
    {
        FryingRecipeSO fryingRecipe = GetFryingRecipeWithInput(kitchenObjectsSO);
        if (fryingRecipe != null)
        {
            return fryingRecipe.output;
        }
        else
        {
            return null;
        }

    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        FryingRecipeSO fryingRecipe = GetFryingRecipeWithInput(kitchenObjectsSO);
        return fryingRecipe != null;
    }

    private FryingRecipeSO GetFryingRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (FryingRecipeSO fryingRecipe in fryingRecipeSOArray)
        {
            if (fryingRecipe.input == kitchenObjectsSO)
            {
                return fryingRecipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (BurningRecipeSO burningRecipe in burningRecipeSOArray)
        {
            if (burningRecipe.input == kitchenObjectsSO)
            {
                return burningRecipe;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
